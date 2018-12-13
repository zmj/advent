package main

import (
	"bufio"
	"fmt"
	"io"
	"regexp"
	"strings"
)

func solve(r io.Reader, w io.Writer) error {
	scanner := bufio.NewScanner(r)
	var parser parser
	game, err := parser.game(scanner)
	if err != nil {
		return fmt.Errorf("parse game: %v", err)
	}
	if err := scanner.Err(); err != nil {
		return fmt.Errorf("read lines: %v", err)
	}
	for i := 1; i <= 20; i++ {
		game.generation()
	}
	sum := 0
	for i, p := range game.pots {
		//fmt.Printf("%v: %v\n", i-game.shifted, p)
		if p {
			sum += i - game.shifted
		}
	}
	_, err = fmt.Fprintf(w, "%v", sum)
	return err
}

type game struct {
	pots    []bool
	rules   []rule
	shifted int
}

func (g *game) generation() {
	next := make([]bool, len(g.pots)+4)
	g.shifted += 2
	for i, j := -2, 0; j < len(next); i, j = i+1, j+1 {
		p := g.slice(i)
		//fmt.Printf("%v: %v\n", j, p)
		for _, rule := range g.rules {
			if n, ok := rule.apply(p); ok {
				next[j] = n
				break
			}
		}
	}
	g.pots = next
}

func (g *game) slice(i int) [5]bool {
	var p [5]bool
	for j := 0; j < len(p); j++ {
		k := i + j - 2
		if k >= 0 && k < len(g.pots) {
			p[j] = g.pots[k]
		}
	}
	return p
}

type rule struct {
	l2, l1, c, r1, r2, n bool
}

func (r rule) apply(p [5]bool) (bool, bool) {
	if p[0] == r.l2 &&
		p[1] == r.l1 &&
		p[2] == r.c &&
		p[3] == r.r1 &&
		p[4] == r.r2 {
		return r.n, true
	}
	return p[2], false
}

type parser struct {
	initState *regexp.Regexp
	ruleDef   *regexp.Regexp
}

func (p *parser) game(scanner *bufio.Scanner) (game, error) {
	var g game
	for scanner.Scan() {
		s := strings.TrimSpace((scanner.Text()))
		if len(g.pots) == 0 {
			pots, err := p.pots(s)
			if err != nil {
				return g, fmt.Errorf("parse pots %v: %v", s, err)
			}
			g.pots = pots
		} else if len(s) == 0 {
			continue
		} else {
			rule, err := p.rule(s)
			if err != nil {
				return g, fmt.Errorf("parse '%v': %v", s, err)
			}
			g.rules = append(g.rules, rule)
		}
	}
	return g, nil
}

func (p *parser) pots(s string) ([]bool, error) {
	var err error
	if p.initState == nil {
		const expr = `initial state: ([.#]+)`
		if p.initState, err = regexp.Compile(expr); err != nil {
			return nil, fmt.Errorf("compile '%v': %v", expr, err)
		}
	}
	matches := p.initState.FindStringSubmatch(s)
	if len(matches) != 2 {
		return nil, fmt.Errorf("expected 2 matches in '%v', found %v", s, len(matches))
	}
	var pots []bool
	for i := 0; i < len(matches[1]); i++ {
		pots = append(pots, isPlant(matches[1][i]))
	}
	return pots, nil
}

func isPlant(c byte) bool {
	if c == '#' {
		return true
	} else if c == '.' {
		return false
	} else {
		panic("unexpected char " + string(c))
	}
}

func (p *parser) rule(s string) (rule, error) {
	var r rule
	var err error
	if p.ruleDef == nil {
		const expr = `([.#]{5}) => ([.#])`
		if p.ruleDef, err = regexp.Compile(expr); err != nil {
			return r, fmt.Errorf("compile '%v': %v", expr, err)
		}
	}
	matches := p.ruleDef.FindStringSubmatch(s)
	if len(matches) != 3 {
		return r, fmt.Errorf("expected 3 matches in '%v', found %v", s, len(matches))
	}
	r.l2, r.l1, r.c, r.r1, r.r2, r.n = isPlant(matches[1][0]), isPlant(matches[1][1]), isPlant(matches[1][2]), isPlant(matches[1][3]), isPlant(matches[1][4]), isPlant(matches[2][0])
	return r, nil
}
