package main

import (
	"bufio"
	"fmt"
	"io"
	"os"
	"regexp"
	"strconv"
	"strings"
)

const testMaxDistance = 32
const realMaxDistance = 10000

var maxDistance = -1

func solve(r io.Reader, w io.Writer) error {
	if w == os.Stdout {
		maxDistance = realMaxDistance
	} else {
		maxDistance = testMaxDistance
	}
	scanner := bufio.NewScanner(r)
	var parser parser
	var destinations []point
	maxX := -1
	maxY := -1
	for scanner.Scan() {
		s := strings.TrimSpace((scanner.Text()))
		p, err := parser.point(s)
		if err != nil {
			return fmt.Errorf("parse '%v': %v", s, err)
		}
		destinations = append(destinations, p)
		if p.x > maxX {
			maxX = p.x
		}
		if p.y > maxY {
			maxY = p.y
		}
	}
	if err := scanner.Err(); err != nil {
		return fmt.Errorf("read lines: %v", err)
	}
	grid := newGrid(maxX+2, maxY+1)
	for _, dest := range destinations {
		queue := []point{dest}
		for len(queue) > 0 {
			p := queue[0]
			queue = queue[1:]
			if grid.visit(p, dest) {
				for _, nb := range grid.neighbors(p) {
					queue = append(queue, nb)
				}
			}
		}
	}
	grid.WriteTo(os.Stdout)
	return nil
}

type loc struct {
	distances     map[point]int
	distanceTotal int
}

type grid struct {
	locs [][]loc
	size point
}

func (g grid) visit(p point, dest point) bool {
	l := &g.locs[p.x][p.y]
	if l.distances == nil {
		l.distances = make(map[point]int)
	}
	if _, exists := l.distances[dest]; exists {
		return false
	}
	d := distance(p, dest)
	l.distances[dest] = d
	l.distanceTotal += d
	fmt.Printf("visit %v %v %v %v\n", p, dest, d, l)
	return d < maxDistance
}

func distance(p, q point) int {
	var dx, dy int
	if p.x > q.x {
		dx = p.x - q.x
	} else {
		dx = q.x - p.x
	}
	if p.y > q.y {
		dy = p.y - q.y
	} else {
		dy = q.y - p.y
	}
	return dx + dy
}

func (g grid) neighbors(p point) []point {
	var nb []point
	if p.x > 0 {
		nb = append(nb, point{p.x - 1, p.y})
	}
	if p.y > 0 {
		nb = append(nb, point{p.x, p.y - 1})
	}
	if p.x+1 < g.size.x {
		nb = append(nb, point{p.x + 1, p.y})
	}
	if p.y+1 < g.size.y {
		nb = append(nb, point{p.x, p.y + 1})
	}
	return nb
}

func (g grid) WriteTo(w io.Writer) (int64, error) {
	for y := 0; y < g.size.y; y++ {
		for x := 0; x < g.size.x; x++ {
			col := g.locs[x][y]
			if col.distanceTotal < maxDistance {
				fmt.Fprintf(w, "%v\t", col.distanceTotal)
			} else {
				fmt.Fprintf(w, ".\t")
			}
		}
		fmt.Fprintf(w, "\n")
	}
	return 0, nil
}

func newGrid(sizeX, sizeY int) grid {
	g := grid{size: point{sizeX, sizeY}}
	for x := 0; x < sizeX; x++ {
		cols := make([]loc, sizeY)
		g.locs = append(g.locs, cols)
	}
	return g
}

type parser struct {
	re *regexp.Regexp
}

type point struct {
	x, y int
}

func (p *parser) point(s string) (point, error) {
	var pt point
	var err error
	if p.re == nil {
		const expr = `(\d+), (\d+)`
		p.re, err = regexp.Compile(expr)
		if err != nil {
			return pt, fmt.Errorf("compile '%v': %v", expr, err)
		}
	}
	matches := p.re.FindStringSubmatch(s)
	if len(matches) != 3 {
		return pt, fmt.Errorf("expected 3 matches on '%v'", s)
	}
	if pt.x, err = strconv.Atoi(matches[1]); err != nil {
		return pt, fmt.Errorf("parse x '%v': %v", matches[1], err)
	}
	if pt.y, err = strconv.Atoi(matches[2]); err != nil {
		return pt, fmt.Errorf("parse y '%v': %v", matches[2], err)
	}
	return pt, nil
}
