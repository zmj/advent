package main

import (
	"bufio"
	"fmt"
	"io"
	"strconv"
	"strings"
)

type node struct {
	parent   *node
	children []*node
	metadata []int
}

func solve(r io.Reader, w io.Writer) error {
	scanner := bufio.NewScanner(r)
	scanner.Split(bufio.ScanWords)
	var head node
	head.build(tokenizer{scanner})
	if err := scanner.Err(); err != nil {
		return fmt.Errorf("read ints: %v", err)
	}
	value := head.value()
	_, err := fmt.Fprintf(w, "%v", value)
	return err
}

func (n *node) value() int {
	value := 0
	if len(n.children) == 0 {
		for _, m := range n.metadata {
			value += m
		}
		return value
	}
	for _, m := range n.metadata {
		idx := m - 1
		if idx >= 0 && idx < len(n.children) {
			value += n.children[idx].value()
		}
	}
	return value
}

func (n *node) build(token tokenizer) {
	childCount := token.next()
	metadataCount := token.next()
	for i := 0; i < childCount; i++ {
		child := &node{parent: n}
		n.children = append(n.children, child)
		child.build(token)
	}
	for i := 0; i < metadataCount; i++ {
		n.metadata = append(n.metadata, token.next())
	}
}

type tokenizer struct {
	*bufio.Scanner
}

func (t tokenizer) next() int {
	if !t.Scan() {
		panic("expected token")
	}
	s := strings.TrimSpace(t.Text())
	i, err := strconv.Atoi(s)
	if err != nil {
		panic(err.Error())
	}
	return i
}
