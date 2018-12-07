package main

import (
	"bufio"
	"fmt"
	"io"
	"os"
	"regexp"
	"sort"
	"strings"
)

const testDelay = 0
const realDelay = 60
const realWorkerCount = 5
const testWorkerCount = 2

var delay int
var workerCount int

func solve(r io.Reader, w io.Writer) error {
	if w == os.Stdout {
		delay = realDelay
		workerCount = realWorkerCount
	} else {
		delay = testDelay
		workerCount = testWorkerCount
	}
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
	workers := make([]worker, workerCount)
	var time int
	for time = 0; ; time++ {
		var idle []*worker
		for i, w := range workers {
			if w.done > time {
				continue
			}
			idle = append(idle, &workers[i])
			if w.doing == nil {
				continue
			}
			for _, d := range w.doing.blocks {
				delete(d.dependsOn, w.doing.name)
				if len(d.dependsOn) == 0 {
					queue = append(queue, d)
				}
			}
			workers[i].doing = nil
		}
		if len(idle) == 0 {
			continue
		}
		if len(queue) == 0 {
			if len(idle) < workerCount {
				continue
			} else {
				break
			}
		}
		sort.Sort(queue)
		for len(queue) > 0 && len(idle) > 0 {
			todo := queue[0]
			queue = queue[1:]
			idle[0].doing = todo
			idle[0].done = time + todo.duration()
			idle = idle[1:]
		}
	}
	_, err := fmt.Fprintf(w, "%v", time)
	return err
}

type worker struct {
	done  int
	doing *step
}

func (s step) duration() int {
	i := s.name - 'A' + 1
	return int(i) + delay
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
