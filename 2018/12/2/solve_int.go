package main

/*
import (
	"bufio"
	"fmt"
	"io"
	"os"
	"regexp"
	"strings"
)

const (
	testMaxGen = 20
	realMaxGen = 50000000000
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
	var gen, maxGen uint64
	if w == os.Stdout {
		maxGen = realMaxGen
	} else {
		maxGen = testMaxGen
	}
	prev := make(map[uint64]uint64)
	prev[game.pots] = 0
	for gen = 1; gen <= maxGen; gen++ {
		game.generation()
		if lastGen, ok := prev[game.pots]; ok {
			fmt.Printf("cycle! %v %v %v %v\n", lastGen, gen, game.pots, game.sum())
			cycle := gen - lastGen
			for gen+cycle <= maxGen {
				gen += cycle
			}
		}
		prev[game.pots] = gen
	}
	_, err = fmt.Fprintf(w, "%v", game.sum())
	return err
}

func (g game) sum() int {
	var shift uint
	p := g.pots
	sum := 0
	for shift = 0; shift < 64; shift++ {
		fmt.Printf("%v: %v\n", shift, p&1)
		if p&1 == 1 {
			sum += int(shift) - 20
		}
		p >>= 1
	}
	return sum
}

type game struct {
	pots    uint64
	rules   map[uint64]bool
	shifted int64
}

func (g *game) generation() {
	p := g.pots
	var next uint64
	const mask = 31
	var shift uint
	for shift = 0; shift < 64; shift++ {
		q := p & mask
		if q > 0 && g.rules[q] {
			next |= 1 << (shift + 2)
		}
		p >>= 1
	}
	g.pots = next
}

type rule struct {
	c uint64
	n bool
}

type parser struct {
	initState *regexp.Regexp
	ruleDef   *regexp.Regexp
}

func (p *parser) game(scanner *bufio.Scanner) (game, error) {
	g := game{rules: make(map[uint64]bool)}
	for scanner.Scan() {
		s := strings.TrimSpace((scanner.Text()))
		if g.pots == 0 {
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
			g.rules[rule.c] = rule.n
		}
	}
	return g, nil
}

func (p *parser) pots(s string) (uint64, error) {
	var err error
	if p.initState == nil {
		const expr = `initial state: ([.#]+)`
		if p.initState, err = regexp.Compile(expr); err != nil {
			return 0, fmt.Errorf("compile '%v': %v", expr, err)
		}
	}
	matches := p.initState.FindStringSubmatch(s)
	if len(matches) != 2 {
		return 0, fmt.Errorf("expected 2 matches in '%v', found %v", s, len(matches))
	}
	var pots uint64
	for i := 0; i < len(matches[1]); i++ {
		if isPlant(matches[1][i]) {
			pots |= 1 << uint(i+20)
		}
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
	for i := 0; i < len(matches[1]); i++ {
		if isPlant(matches[1][i]) {
			r.c |= 1 << uint(i)
		}
	}
	r.n = isPlant(matches[2][0])
	return r, nil
}
*/
