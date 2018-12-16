package main

import (
	"bufio"
	"fmt"
	"io"
	"sort"
)

func solve(r io.Reader, w io.Writer) error {
	scanner := bufio.NewScanner(r)
	game, err := parseGame(scanner)
	if err := scanner.Err(); err != nil {
		return fmt.Errorf("read lines: %v", err)
	}
	if err != nil {
		return fmt.Errorf("parse: %v", err)
	}
	var outcome int
	round := 0
	for {
		//subgame.WriteTo(os.Stdout)
		complete := game.round()
		//fmt.Printf("after round %v (complete? %v)\n", round, complete)
		//game.WriteTo(os.Stdout)
		if complete {
			round++
		} else {
			break
		}
	}
	outcome = game.outcome(round)
	_, err = fmt.Fprint(w, outcome)
	return err
}

func (g *game) round() bool {
	sort.Sort(g.units)
	complete := true
	for _, u := range g.units {
		if u.hp <= 0 {
			continue
		}
		complete = complete && g.move(u)
		g.attack(u)
	}
	var liveUnits units
	for _, u := range g.units {
		if u.hp > 0 {
			liveUnits = append(liveUnits, u)
		}
	}
	g.units = liveUnits
	return complete
}

func (g *game) move(u *unit) bool {
	for _, adj := range g.adjacent(u.x, u.y) {
		other := g.grid[adj.y][adj.x].unit
		if other != nil && other.name != u.name {
			return true
		}
	}
	targets := make(map[point]bool)
	complete := false
	for _, other := range g.units {
		if other.name == u.name || other.hp <= 0 {
			continue
		}
		complete = true
		for _, pt := range g.adjacent(other.x, other.y) {
			loc := g.grid[pt.y][pt.x]
			if loc.terrain == '.' && loc.unit == nil {
				targets[pt] = true
			}
		}
	}
	if len(targets) == 0 {
		return complete
	}
	visited := map[point]int{point{u.x, u.y}: 0}
	paths := [][]point{[]point{point{u.x, u.y}}}
	var solutions [][]point
	for len(paths) > 0 && len(solutions) == 0 {
		var newPaths [][]point
		for _, path := range paths {
			for _, pt := range g.adjacent(path[len(path)-1].x, path[len(path)-1].y) {
				loc := g.grid[pt.y][pt.x]
				if loc.terrain != '.' || loc.unit != nil {
					continue
				}
				if _, exists := visited[pt]; exists {
					continue
				}
				visited[pt] = len(path)
				newPath := make([]point, len(path)+1)
				copy(newPath, path)
				newPath[len(path)] = pt
				newPaths = append(newPaths, newPath)
			}
		}
		paths = newPaths
		for _, path := range paths {
			if targets[path[len(path)-1]] {
				solutions = append(solutions, path)
			}
		}
	}
	if len(solutions) > 0 {
		pt := solutions[0][1]
		//fmt.Printf("move from %v,%v to %v,%v\n", u.x, u.y, pt.x, pt.y)
		g.grid[u.y][u.x].unit = nil
		u.x, u.y = pt.x, pt.y
		g.grid[u.y][u.x].unit = u
	}
	return complete
}

func (g *game) attack(u *unit) {
	var target *unit
	for _, adj := range g.adjacent(u.x, u.y) {
		other := g.grid[adj.y][adj.x].unit
		if other == nil || other.name == u.name {
			continue
		}
		if target == nil || target.hp > other.hp {
			target = other
		}
	}
	if target == nil {
		return
	}
	target.hp -= u.ap
	if target.hp <= 0 {
		g.grid[target.y][target.x].unit = nil
	}
}

type point struct{ x, y int }

func (g game) adjacent(x, y int) []point {
	var adj []point
	if y-1 >= 0 {
		adj = append(adj, point{x, y - 1})
	}
	if x-1 >= 0 {
		adj = append(adj, point{x - 1, y})
	}
	if x+1 < len(g.grid[y]) {
		adj = append(adj, point{x + 1, y})
	}
	if y+1 < len(g.grid) {
		adj = append(adj, point{x, y + 1})
	}
	return adj
}

type game struct {
	grid  [][]loc
	units units
}

func (g game) isOver() bool {
	var goblin, elf bool
	for _, u := range g.units {
		switch u.name {
		case 'G':
			goblin = true
		case 'E':
			elf = true
		}
		if goblin && elf {
			break
		}
	}
	return !goblin || !elf
}

func (g game) outcome(round int) int {
	sumHP := 0
	for _, u := range g.units {
		sumHP += u.hp
	}
	return sumHP * round
}

type loc struct {
	terrain rune
	unit    *unit
}

type unit struct {
	name   rune
	x, y   int
	hp, ap int
}

type units []*unit

func (u units) Len() int      { return len(u) }
func (u units) Swap(i, j int) { u[i], u[j] = u[j], u[i] }
func (u units) Less(i, j int) bool {
	if u[i].y != u[j].y {
		return u[i].y < u[j].y
	}
	return u[i].x < u[j].x
}

func (g game) WriteTo(w io.Writer) (int64, error) {
	var n int64
	for _, row := range g.grid {
		for _, loc := range row {
			var c rune
			if loc.unit != nil {
				c = loc.unit.name
			} else {
				c = loc.terrain
			}
			n2, err := w.Write([]byte(string(c)))
			n += int64(n2)
			if err != nil {
				return n, err
			}
		}
		n2, err := fmt.Fprint(w, "\n")
		n += int64(n2)
		if err != nil {
			return n, err
		}
	}
	for _, u := range g.units {
		n2, err := fmt.Fprintf(w, "%v\n", u)
		n += int64(n2)
		if err != nil {
			return n, err
		}
	}
	return n, nil
}

func parseGame(scanner *bufio.Scanner) (game, error) {
	var g game
	var y int
	for scanner.Scan() {
		s := scanner.Text()
		if len(s) == 0 {
			continue
		}
		var row []loc
		for x, c := range s {
			var l loc
			var u *unit
			switch c {
			case 'G', 'E':
				u = &unit{c, x, y, 200, 3}
				l = loc{'.', u}
			case '.', '#':
				l.terrain = c
			default:
				return g, fmt.Errorf("unexpected charater: %v", string(c))
			}
			row = append(row, l)
			if u != nil {
				g.units = append(g.units, u)
			}
		}
		g.grid = append(g.grid, row)
		y++
	}
	return g, nil
}
