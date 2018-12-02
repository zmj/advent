package main

import (
	"advent/tableTest"
	"testing"
)

var tests = []tableTest.Test{
	tableTest.Test{
		Input:  `+1`,
		Output: `1`,
	},
	tableTest.Test{
		Input: `+1
		-2
		+3
		+1`,
		Output: `3`,
	},
	tableTest.Test{
		Input: `+1
		+1
		+1`,
		Output: `3`,
	},
	tableTest.Test{
		Input: `+1
		+1
		-2`,
		Output: `0`,
	},
	tableTest.Test{
		Input: `-1
		-2
		-3`,
		Output: `-6`,
	},
}

func TestSolve(t *testing.T) {
	tableTest.Run(solve, tests, t)
}
