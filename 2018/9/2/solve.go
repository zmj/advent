package main

import (
	"bufio"
	"fmt"
	"io"
	"regexp"
	"strconv"
	"strings"
)

func solve(r io.Reader, w io.Writer) error {
	scanner := bufio.NewScanner(r)
	var parser parser
	var game game
	var err error
	for scanner.Scan() {
		s := strings.TrimSpace((scanner.Text()))
		if game, err = parser.game(s); err != nil {
			return fmt.Errorf("parse '%v': %v", s, err)
		}
	}
	if err := scanner.Err(); err != nil {
		return fmt.Errorf("read lines: %v", err)
	}
	game.play()
	_, err = fmt.Fprintf(w, "%v", game.highScore())
	return err
}

func (g *game) play() {
	zero := &marble{}
	zero.clock = zero
	zero.counter = zero
	g.current = zero
	for i, p := 1, 0; i <= g.maxMarble; i, p = i+1, (p+1)%len(g.playerScores) {
		m := &marble{value: i}
		if i%23 == 0 {
			g.twentyThree(p, m)
		} else {
			g.place(m)
		}
	}
}

func (g *game) place(m *marble) {
	counter := g.current.clock
	clock := counter.clock
	m.counter = counter
	m.clock = clock
	counter.clock = m
	clock.counter = m
	g.current = m
}

func (g *game) twentyThree(player int, m *marble) {
	g.playerScores[player] += m.value
	remove := g.current
	for i := 0; i < 7; i++ {
		remove = remove.counter
	}
	remove.counter.clock = remove.clock
	remove.clock.counter = remove.counter
	g.playerScores[player] += remove.value
	g.current = remove.clock
}

type game struct {
	maxMarble    int
	playerScores []int
	current      *marble
}

type marble struct {
	value          int
	clock, counter *marble
}

func (g game) highScore() int {
	score := -1
	for _, p := range g.playerScores {
		if p > score {
			score = p
		}
	}
	return score
}

type parser struct {
	re *regexp.Regexp
}

func (p *parser) game(s string) (game, error) {
	var g game
	var err error
	if p.re == nil {
		const expr = `(\d+) players; last marble is worth (\d+) points`
		if p.re, err = regexp.Compile(expr); err != nil {
			return g, fmt.Errorf("compile '%v': %v", expr, err)
		}
	}
	matches := p.re.FindStringSubmatch(s)
	if len(matches) != 3 {
		return g, fmt.Errorf("expected 3 matches in '%v' found %v", s, len(matches))
	}
	var playerCount int
	if playerCount, err = strconv.Atoi(matches[1]); err != nil {
		return g, fmt.Errorf("parse playerCount '%v': %v", matches[1], err)
	}
	g.playerScores = make([]int, playerCount)
	if g.maxMarble, err = strconv.Atoi(matches[2]); err != nil {
		return g, fmt.Errorf("parse maxMarble '%v': %v", matches[2], err)
	}
	return g, nil
}
