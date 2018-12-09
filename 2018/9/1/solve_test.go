package main

import (
	"advent/tableTest"
	"testing"
)

var tests = []tableTest.Test{
	tableTest.Test{
		Input:  `9 players; last marble is worth 25 points`,
		Output: `32`,
	},
	tableTest.Test{
		Input:  `10 players; last marble is worth 1618 points`,
		Output: `8317`,
	},
	tableTest.Test{
		Input:  `13 players; last marble is worth 7999 points`,
		Output: `146373`,
	},
	tableTest.Test{
		Input:  `17 players; last marble is worth 1104 points`,
		Output: `2764`,
	},
	tableTest.Test{
		Input:  `21 players; last marble is worth 6111 points`,
		Output: `54718`,
	},
	tableTest.Test{
		Input:  `30 players; last marble is worth 5807 points`,
		Output: `37305`,
	},
}

func TestSolve(t *testing.T) {
	tableTest.Run(solve, tests, t)
}
