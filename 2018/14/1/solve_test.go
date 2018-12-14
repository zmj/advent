package main

import (
	"advent/tableTest"
	"testing"
)

var tests = []tableTest.Test{
	tableTest.Test{
		Input:  `9`,
		Output: `5158916779`,
	},
	tableTest.Test{
		Input:  `5`,
		Output: `0124515891`,
	},
	tableTest.Test{
		Input:  `18`,
		Output: `9251071085`,
	},
	tableTest.Test{
		Input:  `2018`,
		Output: `5941429882`,
	},
}

func TestSolve(t *testing.T) {
	tableTest.Run(solve, tests, t)
}
