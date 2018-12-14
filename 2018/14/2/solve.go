package main

import (
	"bufio"
	"fmt"
	"io"
	"strconv"
	"strings"
)

func solve(r io.Reader, w io.Writer) error {
	scanner := bufio.NewScanner(r)
	var target []int
	for scanner.Scan() {
		s := strings.TrimSpace((scanner.Text()))
		for _, c := range s {
			i, err := strconv.Atoi(string(c))
			if err != nil {
				return fmt.Errorf("parse '%v': %v", string(c), err)
			}
			target = append(target, i)
		}
	}
	if err := scanner.Err(); err != nil {
		return fmt.Errorf("read lines: %v", err)
	}
	game := game{
		recipes: []int{3, 7},
		elves:   []int{0, 1},
	}
	var targetStart int
step:
	for {
		game.step()
		if len(game.recipes) < len(target)*2 {
			continue
		}
	search:
		for start := len(game.recipes) - len(target)*2; len(game.recipes)-start >= len(target); start++ {
			compare := game.recipes[start : start+len(target)]
			//fmt.Printf("compare: %v %v start:%v\n", target, compare, start)
			for i := 0; i < len(target); i++ {
				if compare[i] != target[i] {
					continue search
				}
			}
			targetStart = start
			break step
		}
	}
	_, err := fmt.Fprintf(w, "%v", targetStart)
	return err
}

type game struct {
	elves   []int
	recipes []int
}

func (g *game) step() {
	var sum int
	for _, elf := range g.elves {
		sum += g.recipes[elf]
	}
	for _, c := range strconv.Itoa(sum) {
		r, _ := strconv.Atoi(string(c))
		g.recipes = append(g.recipes, r)
	}
	for i := 0; i < len(g.elves); i++ {
		g.elves[i] = (g.elves[i] + 1 + g.recipes[g.elves[i]]) % len(g.recipes)
	}
}
