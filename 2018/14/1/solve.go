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
	var targetCount int
	for scanner.Scan() {
		s := strings.TrimSpace((scanner.Text()))
		i, err := strconv.Atoi(s)
		if err != nil {
			return fmt.Errorf("parse '%v': %v", s, err)
		}
		targetCount = i
	}
	if err := scanner.Err(); err != nil {
		return fmt.Errorf("read lines: %v", err)
	}
	game := game{
		recipes: []int{3, 7},
		elves:   []int{0, 1},
	}
	for len(game.recipes) < targetCount+10 {
		game.step()
	}
	nextRecipes := ""
	for i := targetCount; i < targetCount+10; i++ {
		nextRecipes += strconv.Itoa(game.recipes[i])
	}
	_, err := fmt.Fprintf(w, "%v", nextRecipes)
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
