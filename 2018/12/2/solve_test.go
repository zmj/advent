package main

import (
	"advent/tableTest"
	"testing"
)

var tests = []tableTest.Test{
	tableTest.Test{
		Input: `initial state: #..#.#..##......###...###

		...## => #
		..#.. => #
		.#... => #
		.#.#. => #
		.#.## => #
		.##.. => #
		.#### => #
		#.#.# => #
		#.### => #
		##.#. => #
		##.## => #
		###.. => #
		###.# => #
		####. => #`,
		Output: `325`,
	},
}

func TestSolve(t *testing.T) {
	tableTest.Run(solve, tests, t)
}
