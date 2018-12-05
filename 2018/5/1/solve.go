package main

import (
	"bufio"
	"fmt"
	"io"
	"unicode"
)

func solve(r io.Reader, w io.Writer) error {
	rdr := bufio.NewReader(r)
	var polymer []rune
	for {
		c, _, err := rdr.ReadRune()
		if err == io.EOF {
			break
		} else if err != nil {
			return fmt.Errorf("read: %v", err)
		}
		if len(polymer) == 0 {
			polymer = append(polymer, c)
			continue
		}
		d := polymer[len(polymer)-1]
		if react(c, d) {
			polymer = polymer[:len(polymer)-1]
		} else {
			polymer = append(polymer, c)
		}
	}
	_, err := fmt.Fprintf(w, "%v", len(polymer))
	return err
}

func react(c, d rune) bool {
	cu := unicode.ToUpper(c)
	du := unicode.ToUpper(d)
	return cu == du &&
		((c == cu && d != du) ||
			(c != cu && d == du))
}
