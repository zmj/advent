package main

import (
	"bufio"
	"fmt"
	"io"
	"strings"
)

func solve(r io.Reader, w io.Writer) error {
	scanner := bufio.NewScanner(r)
	hasTwo := 0
	hasThree := 0
	for scanner.Scan() {
		s := strings.TrimSpace(scanner.Text())
		rpt := count(s)
		if rpt.hasTwo {
			hasTwo++
		}
		if rpt.hasThree {
			hasThree++
		}
	}
	if err := scanner.Err(); err != nil {
		return fmt.Errorf("read lines: %v", err)
	}
	checksum := hasTwo * hasThree
	_, err := fmt.Fprintf(w, "%v", checksum)
	return err
}

type repeats struct {
	hasTwo   bool
	hasThree bool
}

func count(id string) repeats {
	counts := make(map[rune]int)
	for _, c := range id {
		counts[c]++
	}
	var rpt repeats
	for _, n := range counts {
		if n == 2 {
			rpt.hasTwo = true
		}
		if n == 3 {
			rpt.hasThree = true
		}
	}
	return rpt
}
