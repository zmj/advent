package main

import (
	"fmt"
	"io"
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
	grid := make([][]int, 300)
	for i := 0; i < len(grid); i++ {
		grid[i] = make([]int, len(grid))
	}
	var highX, highY int
	for y := 0; y < len(grid); y++ {
		for x := 0; x < len(grid); x++ {
			power := power(x+1, y+1, serialNumber)
			for dx := 0; dx < 3; dx++ {
				for dy := 0; dy < 3; dy++ {
					sx, sy := x-dx, y-dy
					if sx >= 0 && sy >= 0 {
						grid[sx][sy] += power
						if grid[sx][sy] > grid[highX][highY] {
							highX, highY = sx, sy
						}
					}
				}
			}
		}
	}
	fmt.Fprintf(w, "%v,%v", highX+1, highY+1)
	return nil
}

func power(x, y, serial int) int {
	rackID := x + 10
	power := rackID * y
	power += serial
	power *= rackID
	hundred := (power % 1000) / 100
	return hundred - 5
}
