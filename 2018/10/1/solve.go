package main

import (
	"bufio"
	"fmt"
	"io"
	"math"
	"os"
	"regexp"
	"strconv"
	"strings"
)

func solve(r io.Reader, w io.Writer) error {
	scanner := bufio.NewScanner(r)
	var parser parser
	var points []point
	for scanner.Scan() {
		s := strings.TrimSpace((scanner.Text()))
		pt, err := parser.point(s)
		if err != nil {
			return fmt.Errorf("parse '%v': %v", s, err)
		}
		points = append(points, pt)
	}
	if err := scanner.Err(); err != nil {
		return fmt.Errorf("read lines: %v", err)
	}
	highScore := -1
	for t := 1; ; t++ {
		for i := 0; i < len(points); i++ {
			pt := &points[i]
			pt.x += pt.dx
			pt.y += pt.dy
		}
		score := score(points)
		if score > highScore {
			highScore = score
			fmt.Printf("print grid %v (score %v)\n", t, score)
			ok, err := printGrid(os.Stdout, points)
			if err != nil {
				return fmt.Errorf("print grid %v: %v", points, err)
			}
			if !ok {
				fmt.Fprintf(w, "skipped printing grid %v (score %v): too large", t, score)
			}
		}
	}
	return nil
}

type point struct {
	x, y, dx, dy int
}

func score(points []point) int {
	score := 0
	for i, pi := range points {
		for j, pj := range points {
			if i == j {
				continue
			}
			var x, y int
			if pi.x > pj.x {
				x = pi.x - pj.x
			} else {
				x = pj.x - pi.x
			}
			if pi.y > pj.y {
				y = pi.y - pj.y
			} else {
				y = pj.y - pi.y
			}
			distance := x + y
			if distance == 1 || distance == 2 {
				score += distance
			}
		}
	}
	return score
}

func printGrid(w io.Writer, points []point) (bool, error) {
	xMin, yMin := math.MaxInt32, math.MaxInt32
	xMax, yMax := math.MinInt32, math.MinInt32
	for _, pt := range points {
		if pt.x < xMin {
			xMin = pt.x
		}
		if pt.x > xMax {
			xMax = pt.x
		}
		if pt.y < yMin {
			yMin = pt.y
		}
		if pt.y > yMax {
			yMax = pt.y
		}
	}
	const printSizeMax = 200
	if xMax-xMin > printSizeMax || yMax-yMin > printSizeMax {
		return false, nil
	}
	for y := yMin; y <= yMax; y++ {
		var err error
		for x := xMin; x <= xMax; x++ {
			exists := false
			for _, pt := range points {
				if x == pt.x && y == pt.y {
					exists = true
					break
				}
			}
			if exists {
				_, err = fmt.Fprint(w, "*")
			} else {
				_, err = fmt.Fprint(w, " ")
			}
			if err != nil {
				return false, err
			}
		}
		_, err = fmt.Fprint(w, "\n")
		if err != nil {
			return false, err
		}
	}
	return true, nil
}

type parser struct {
	re *regexp.Regexp
}

func (p *parser) point(s string) (point, error) {
	var pt point
	var err error
	if p.re == nil {
		const expr = `position=< *([0-9-]+), +([0-9-]+)> velocity=< *([0-9-]+), +([0-9-]+)>`
		if p.re, err = regexp.Compile(expr); err != nil {
			return pt, fmt.Errorf("compile '%v': %v", expr, err)
		}
	}
	matches := p.re.FindStringSubmatch(s)
	if len(matches) != 5 {
		return pt, fmt.Errorf("expected 5 matches in '%v', found %v", s, len(matches))
	}
	if pt.x, err = strconv.Atoi(matches[1]); err != nil {
		return pt, fmt.Errorf("parse x '%v': %v", matches[1], err)
	}
	if pt.y, err = strconv.Atoi(matches[2]); err != nil {
		return pt, fmt.Errorf("parse y '%v': %v", matches[2], err)
	}
	if pt.dx, err = strconv.Atoi(matches[3]); err != nil {
		return pt, fmt.Errorf("parse dx '%v': %v", matches[3], err)
	}
	if pt.dy, err = strconv.Atoi(matches[4]); err != nil {
		return pt, fmt.Errorf("parse dy '%v': %v", matches[4], err)
	}
	return pt, nil
}
