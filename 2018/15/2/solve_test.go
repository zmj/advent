package main

import (
	"advent/tableTest"
	"testing"
)

var tests = []tableTest.Test{
	tableTest.Test{
		Input: `
#######
#.G...#
#...EG#
#.#.#G#
#..G#E#
#.....#
#######`,
		Output: `4988`,
	},
	tableTest.Test{
		Input: `
#######
#E..EG#
#.#G.E#
#E.##E#
#G..#.#
#..E#.#
#######`,
		Output: `31284`,
	},
	tableTest.Test{
		Input: `
#######
#E.G#.#
#.#G..#
#G.#.G#
#G..#.#
#...E.#
#######`,
		Output: `3478`,
	},
	tableTest.Test{
		Input: `
#######
#.E...#
#.#..G#
#.###.#
#E#G#G#
#...#G#
#######`,
		Output: `6474`,
	},
	tableTest.Test{
		Input: `
#########
#G......#
#.E.#...#
#..##..G#
#...##..#
#...#...#
#.G...G.#
#.....G.#
#########`,
		Output: `1140`,
	},
}

func TestSolve(t *testing.T) {
	tableTest.Run(solve, tests, t)
}
