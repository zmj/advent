package main

import (
	"bufio"
	"fmt"
	"io"
	"math"
	"regexp"
	"strconv"
	"strings"
)

func solve(r io.Reader, w io.Writer) error {
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
	var queue []point
	for _, p := range destinations {
		grid.locs[p.x][p.y] = loc{
			visited:  true,
			closest:  []point{p},
			distance: 0,
		}
		for _, nb := range grid.neighbors(p) {
			queue = append(queue, nb)
		}
	}
	for len(queue) > 0 {
		p := queue[0]
		queue = queue[1:]
		if grid.visit(p) {
			for _, nb := range grid.neighbors(p) {
				queue = append(queue, nb)
			}
		}
	}
	areas := make(map[point]int)
	for _, p := range destinations {
		areas[p] = 0
	}
	for x := 0; x < grid.size.x; x++ {
		for y := 0; y < grid.size.y; y++ {
			l := grid.locs[x][y]
			if len(l.closest) > 1 {
				continue
			}
			c := l.closest[0]
			if x == 0 || y == 0 || x == grid.size.x-1 || y == grid.size.y-1 {
				delete(areas, c)
				continue
			}
			if _, exists := areas[c]; exists {
				areas[c]++
			}
		}
	}
	maxArea := -1
	for _, a := range areas {
		if a > maxArea {
			maxArea = a
		}
	}
	_, err := fmt.Fprintf(w, "%v", maxArea)
	return err
}

type loc struct {
	visited  bool
	closest  []point
	distance int
}

type grid struct {
	locs [][]loc
	size point
}

func (g grid) visit(p point) bool {
	l := &g.locs[p.x][p.y]
	if l.visited {
		return false
	}
	l.visited = true
	l.distance = math.MaxInt32
	for _, q := range g.neighbors(p) {
		nb := g.locs[q.x][q.y]
		if !nb.visited {
			continue
		}
		if nb.distance+1 < l.distance {
			l.closest = nb.closest
			l.distance = nb.distance + 1
		} else if nb.distance+1 == l.distance {
			m := make(map[point]bool)
			for _, c := range l.closest {
				m[c] = true
			}
			for _, c := range nb.closest {
				if !m[c] {
					l.closest = append(l.closest, c)
					m[c] = true
				}
			}
		}
	}
	return true
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
			if col.distance == 0 {
				fmt.Fprintf(w, "+\t")
			} else if len(col.closest) > 1 {
				fmt.Fprintf(w, "*\t")
			} else {
				fmt.Fprintf(w, "%v\t", col.closest[0])
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
