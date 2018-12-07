package main

import (
	"bufio"
	"fmt"
	"io"
	"regexp"
	"sort"
	"strings"
)

func solve(r io.Reader, w io.Writer) error {
	scanner := bufio.NewScanner(r)
	var parser parser
	steps := make(map[rune]*step)
	for scanner.Scan() {
		s := strings.TrimSpace((scanner.Text()))
		r, err := parser.rule(s)
		if err != nil {
			return fmt.Errorf("parse '%v': %v", s, err)
		}
		b, exists := steps[r.step]
		if !exists {
			b = &step{name: r.step, dependsOn: make(map[rune]*step)}
			steps[r.step] = b
		}
		d, exists := steps[r.dependsOn]
		if !exists {
			d = &step{name: r.dependsOn, dependsOn: make(map[rune]*step)}
			steps[r.dependsOn] = d
		}
		d.dependsOn[b.name] = b
		b.blocks = append(b.blocks, d)
	}
	if err := scanner.Err(); err != nil {
		return fmt.Errorf("read lines: %v", err)
	}
	var queue queue
	for _, step := range steps {
		if len(step.dependsOn) == 0 {
			queue = append(queue, step)
		}
	}
	sort.Sort(queue)
	for len(queue) > 0 {
		step := queue[0]
		queue = queue[1:]
		if _, err := fmt.Fprint(w, string(step.name)); err != nil {
			return fmt.Errorf("printf: %v", err)
		}
		for _, d := range step.blocks {
			delete(d.dependsOn, step.name)
			if len(d.dependsOn) == 0 {
				queue = append(queue, d)
			}
		}
		sort.Sort(queue)
	}
	return nil
}

type step struct {
	name      rune
	blocks    []*step
	dependsOn map[rune]*step
}

type queue []*step

func (q queue) Len() int           { return len(q) }
func (q queue) Swap(i, j int)      { q[i], q[j] = q[j], q[i] }
func (q queue) Less(i, j int) bool { return q[i].name < q[j].name }

type rule struct {
	step, dependsOn rune
}

type parser struct {
	re *regexp.Regexp
}

func (p *parser) rule(s string) (rule, error) {
	var r rule
	var err error
	if p.re == nil {
		const expr = `Step (.) must be finished before step (.) can begin.`
		p.re, err = regexp.Compile(expr)
		if err != nil {
			return r, fmt.Errorf("compile '%v': %v", expr, err)
		}
	}
	matches := p.re.FindStringSubmatch(s)
	if len(matches) != 3 {
		return r, fmt.Errorf("expected 3 matches in '%v', found %v", s, len(matches))
	}
	r.step = []rune(matches[1])[0]
	r.dependsOn = []rune(matches[2])[0]
	return r, nil
}
