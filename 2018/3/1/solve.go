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
	var claims []claim
	var parser *parser
	for scanner.Scan() {
		s := strings.TrimSpace(scanner.Text())
		c, err := parser.claim(s)
		if err != nil {
			return fmt.Errorf("parse '%v': %v", s, err)
		}
		claims = append(claims, c)
	}
	if err := scanner.Err(); err != nil {
		return fmt.Errorf("read lines: %v", err)
	}
	const size = 1024
	overlap := 0
	for x := 0; x < size; x++ {
		for y := 0; y < size; y++ {
			contained := -1
			for _, c := range claims {
				if c.contains(x, y) {
					if contained != -1 {
						overlap++
						break
					}
					contained = c.id
				}
			}
		}
	}
	_, err := fmt.Fprintf(w, "%v", overlap)
	return err
}

type claim struct {
	id, left, top, width, height int
}

func (c *claim) contains(x, y int) bool {
	return x >= c.left &&
		x < c.left+c.width &&
		y >= c.top &&
		y < c.top+c.height
}

type parser struct {
	re *regexp.Regexp
}

func (p *parser) claim(s string) (claim, error) {
	re, err := p.regexp()
	var c claim
	if err != nil {
		return c, fmt.Errorf("get regexp: %v", err)
	}
	matches := re.FindStringSubmatch(s)
	if len(matches) != 6 {
		return c, fmt.Errorf("expected 6 matches, found %v", len(matches))
	}
	if c.id, err = strconv.Atoi(matches[1]); err != nil {
		return c, fmt.Errorf("parse id '%v': %v", matches[1], err)
	}
	if c.left, err = strconv.Atoi(matches[2]); err != nil {
		return c, fmt.Errorf("parse left '%v': %v", matches[2], err)
	}
	if c.top, err = strconv.Atoi(matches[3]); err != nil {
		return c, fmt.Errorf("parse top '%v': %v", matches[3], err)
	}
	if c.width, err = strconv.Atoi(matches[4]); err != nil {
		return c, fmt.Errorf("parse width '%v': %v", matches[4], err)
	}
	if c.height, err = strconv.Atoi(matches[5]); err != nil {
		return c, fmt.Errorf("parse height '%v': %v", matches[5], err)
	}
	return c, nil
}

func (p *parser) regexp() (*regexp.Regexp, error) {
	if p == nil {
		p = &parser{}
	}
	if p.re != nil {
		return p.re, nil
	}
	const expr = `#(\d)+ @ (\d+),(\d+): (\d+)x(\d+)`
	re, err := regexp.Compile(expr)
	if err != nil {
		return nil, fmt.Errorf("compile '%v': %v", expr, err)
	}
	p.re = re
	return re, nil
}
