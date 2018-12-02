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
	var freq int64
	for scanner.Scan() {
		s := strings.TrimSpace((scanner.Text()))
		i, err := strconv.Atoi(s)
		if err != nil {
			return fmt.Errorf("parse '%v': %v", s, err)
		}
		freq += int64(i)
	}
	if err := scanner.Err(); err != nil {
		return fmt.Errorf("read lines: %v", err)
	}
	_, err := fmt.Fprintf(w, "%v", freq)
	return err
}
