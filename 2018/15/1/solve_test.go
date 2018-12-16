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
		Output: `27730`,
	},
	tableTest.Test{
		Input: `
#######
#G..#E#
#E#E.E#
#G.##.#
#...#E#
#...E.#
#######`,
		Output: `36334`,
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
		Output: `39514`,
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
		Output: `27755`,
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
		Output: `28944`,
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
		Output: `18740`,
	},
}

func TestSolve(t *testing.T) {
	tableTest.Run(solve, tests, t)
}
