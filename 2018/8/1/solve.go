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
	sum := head.sumMetadata()
	_, err := fmt.Fprintf(w, "%v", sum)
	return err
}

func (n *node) sumMetadata() int {
	sum := 0
	for _, m := range n.metadata {
		sum += m
	}
	for _, child := range n.children {
		sum += child.sumMetadata()
	}
	return sum
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
