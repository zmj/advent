package main

import (
	"bufio"
	"fmt"
	"io"
	"math"
	"unicode"
)

func solve(r io.Reader, w io.Writer) error {
	rdr := bufio.NewReader(r)
	var unfiltered polymer
	filtered := make(map[rune]*polymer)
	for {
		c, _, err := rdr.ReadRune()
		if err == io.EOF {
			break
		} else if err != nil {
			return fmt.Errorf("read: %v", err)
		}
		cu := unicode.ToUpper(c)
		if _, exists := filtered[cu]; !exists {
			p := make([]rune, len(unfiltered.p))
			copy(p, unfiltered.p)
			filtered[cu] = &polymer{p}
		}
		unfiltered.add(c)
		for fu, p := range filtered {
			if cu == fu {
				continue
			}
			p.add(c)
		}
	}
	min := math.MaxInt32
	for _, p := range filtered {
		if len(p.p) < min {
			min = len(p.p)
		}
	}
	_, err := fmt.Fprintf(w, "%v", min)
	return err
}

type polymer struct {
	p []rune
}

func (p *polymer) add(c rune) {
	if len(p.p) == 0 {
		p.p = append(p.p, c)
		return
	}
	d := p.p[len(p.p)-1]
	if react(c, d) {
		p.p = p.p[:len(p.p)-1]
	} else {
		p.p = append(p.p, c)
	}
}

func react(c, d rune) bool {
	cu := unicode.ToUpper(c)
	du := unicode.ToUpper(d)
	return cu == du &&
		((c == cu && d != du) ||
			(c != cu && d == du))
}
