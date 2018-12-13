package main

import (
	"bufio"
	"fmt"
	"io"
	"sort"
)

func solve(r io.Reader, w io.Writer) error {
	scanner := bufio.NewScanner(r)
	grid, err := parse(scanner)
	if err != nil {
		return fmt.Errorf("parse: %v", err)
	}
	var crashed *cart
outer:
	for tick := 0; ; tick++ {
		//grid.WriteTo(os.Stdout)
		sort.Sort(grid.carts)
		for _, cart := range grid.carts {
			ok := grid.move(cart)
			if !ok {
				crashed = cart
				break outer
			}
		}
	}
	if err := scanner.Err(); err != nil {
		return fmt.Errorf("read lines: %v", err)
	}
	_, err = fmt.Fprintf(w, "%v,%v", crashed.x, crashed.y)
	return err
}

func (g grid) move(c *cart) bool {
	oldLoc := &g.tracks[c.y][c.x]
	if oldLoc.track == '+' {
		c.turn()
	}
	switch c.facing {
	case '<':
		c.x--
	case '>':
		c.x++
	case '^':
		c.y--
	case 'v':
		c.y++
	}
	newLoc := &g.tracks[c.y][c.x]
	if newLoc.cart != nil {
		return false
	}
	if newLoc.track == ' ' {
		panic("wheeeee")
	}
	oldLoc.cart = nil
	newLoc.cart = c
	if newLoc.track == '\\' || newLoc.track == '/' {
		c.curve(newLoc.track)
	}
	return true
}

func (c *cart) turn() {
	switch {
	case c.nextTurn == turnStraight:
	case c.nextTurn == turnLeft:
		switch c.facing {
		case '>':
			c.facing = '^'
		case '<':
			c.facing = 'v'
		case '^':
			c.facing = '<'
		case 'v':
			c.facing = '>'
		default:
			panic("unexpected facing " + string(c.facing))
		}
	case c.nextTurn == turnRight:
		switch c.facing {
		case '>':
			c.facing = 'v'
		case '<':
			c.facing = '^'
		case '^':
			c.facing = '>'
		case 'v':
			c.facing = '<'
		default:
			panic("unexpected facing " + string(c.facing))
		}
	default:
		panic("unexpected nextTurn " + string(c.nextTurn))
	}
	c.nextTurn = (c.nextTurn + 1) % 3
}

func (c *cart) curve(t rune) {
	switch t {
	case '/':
		switch c.facing {
		case '>':
			c.facing = '^'
		case '<':
			c.facing = 'v'
		case '^':
			c.facing = '>'
		case 'v':
			c.facing = '<'
		default:
			panic("unexpected facing " + string(c.facing))
		}
	case '\\':
		switch c.facing {
		case '>':
			c.facing = 'v'
		case '<':
			c.facing = '^'
		case '^':
			c.facing = '<'
		case 'v':
			c.facing = '>'
		default:
			panic("unexpected facing " + string(c.facing))
		}
	}
}

type loc struct {
	track rune
	cart  *cart
}

type turn int

const (
	turnLeft = iota
	turnStraight
	turnRight
)

type cart struct {
	x, y     int
	facing   rune
	nextTurn turn
}

type carts []*cart

func (c carts) Len() int      { return len(c) }
func (c carts) Swap(i, j int) { c[i], c[j] = c[j], c[i] }
func (c carts) Less(i, j int) bool {
	if c[i].y != c[j].y {
		return c[i].y < c[j].y
	}
	return c[i].x < c[j].x
}

type grid struct {
	tracks [][]loc
	carts  carts
}

func parse(scanner *bufio.Scanner) (grid, error) {
	var g grid
	var y int
	for scanner.Scan() {
		s := (scanner.Text())
		if len(s) == 0 {
			continue
		}
		var row []loc
		for x, t := range s {
			var loc loc
			var c *cart
			switch t {
			case '<':
				c = &cart{x: x, y: y, facing: '<'}
				loc.track = '-'
			case '>':
				c = &cart{x: x, y: y, facing: '>'}
				loc.track = '-'
			case '^':
				c = &cart{x: x, y: y, facing: '^'}
				loc.track = '|'
			case 'v':
				c = &cart{x: x, y: y, facing: 'v'}
				loc.track = '|'
			case '-', '|', '\\', '/', '+', ' ':
				loc.track = t
			default:
				return g, fmt.Errorf("unexpected char '%v'", string(t))
			}
			if c != nil {
				g.carts = append(g.carts, c)
				loc.cart = c
			}
			row = append(row, loc)
		}
		g.tracks = append(g.tracks, row)
		y++
	}
	return g, nil
}

func (g grid) WriteTo(w io.Writer) (int64, error) {
	var n int64
	for _, row := range g.tracks {
		for _, loc := range row {
			var c rune
			if loc.cart != nil {
				c = loc.cart.facing
			} else {
				c = loc.track
			}
			if n2, err := w.Write([]byte(string(c))); err != nil {
				return n + int64(n2), err
			}
			n++
		}
		if n2, err := w.Write([]byte{'\n'}); err != nil {
			return n + int64(n2), err
		}
		n++
	}
	return n, nil
}
