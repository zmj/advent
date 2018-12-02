package main

import (
	"bufio"
	"fmt"
	"io"
	"strconv"
	"strings"
)

func solve(r io.Reader, w io.Writer) error {
	scanner := bufio.NewScanner(r)
	var changes []int
	for scanner.Scan() {
		s := strings.TrimSpace(scanner.Text())
		i, err := strconv.Atoi(s)
		if err != nil {
			return fmt.Errorf("parse '%v': %v", s, err)
		}
		changes = append(changes, i)
	}
	if err := scanner.Err(); err != nil {
		return fmt.Errorf("read lines: %v", err)
	}
	freqs := make(map[int64]bool)
	var freq int64
	freqs[freq] = true
outer:
	for {
		for _, c := range changes {
			freq += int64(c)
			if _, exists := freqs[freq]; exists {
				break outer
			}
			freqs[freq] = true
		}
	}
	_, err := fmt.Fprintf(w, "%v", freq)
	return err
}
