package main

import (
	"bufio"
	"fmt"
	"io"
	"strings"
)

func solve(r io.Reader, w io.Writer) error {
	scanner := bufio.NewScanner(r)
	var ids [][]rune
	for scanner.Scan() {
		s := strings.TrimSpace(scanner.Text())
		ids = append(ids, []rune(s))
	}
	if err := scanner.Err(); err != nil {
		return fmt.Errorf("read lines: %v", err)
	}
	for _, a := range ids {
		for _, b := range ids {
			i, ok := diff1(a, b)
			if !ok {
				continue
			}
			_, err := fmt.Fprintf(w, "%v%v", string(a[:i]), string(a[i+1:]))
			return err
		}
	}
	return fmt.Errorf("no match found")
}

func diff1(a, b []rune) (int, bool) {
	if len(a) != len(b) {
		return -1, false
	}
	d := -1
	for i := 0; i < len(a); i++ {
		if a[i] != b[i] {
			if d != -1 {
				return -1, false
			}
			d = i
		}
	}
	return d, d != -1
}
