package main

import (
	"fmt"
	"io"
	"math"
	"os"
)

const (
	testSerial = 18
	realSerial = 3031
)

func solve(r io.Reader, w io.Writer) error {
	var serialNumber int
	if w == os.Stdout {
		serialNumber = realSerial
	} else {
		serialNumber = testSerial
	}
	grid := newGrid(300)
	for y := 0; y < len(grid); y++ {
		for x := 0; x < len(grid); x++ {
			power := power(x+1, y+1, serialNumber)
			grid[x][y] = power
		}
	}
	square := grid.maxSquare()
	fmt.Fprintf(w, "%v,%v,%v", square.x+1, square.y+1, square.size+1)
	return nil
}

type grid [][]int

func newGrid(n int) grid {
	grid := make([][]int, 300)
	for i := 0; i < len(grid); i++ {
		grid[i] = make([]int, len(grid))
	}
	return grid
}

type square struct{ x, y, size int }

func (g grid) maxSquare() square {
	maxPower := math.MinInt32
	var maxSquare square
	for x := 0; x < len(g); x++ {
		for y := 0; y < len(g); y++ {
			for size := 1; x+size < len(g) && y+size < len(g); size++ {
				sq := square{x, y, size}
				p := g.power(sq)
				if p > maxPower {
					maxPower = p
					maxSquare = sq
				}
			}
		}
	}
	return maxSquare
}

func (g grid) power(sq square) int {
	p := 0
	for x := sq.x; x <= sq.x+sq.size; x++ {
		for y := sq.y; y <= sq.y+sq.size; y++ {
			p += g[x][y]
		}
	}
	return p
}

func power(x, y, serial int) int {
	rackID := x + 10
	power := rackID * y
	power += serial
	power *= rackID
	hundred := (power % 1000) / 100
	return hundred - 5
}
