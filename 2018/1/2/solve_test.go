package main

import (
	"advent/tableTest"
	"testing"
)

var tests = []tableTest.Test{
	tableTest.Test{
		Input: `+1
		-2
		+3
		+1`,
		Output: `2`,
	},
	tableTest.Test{
		Input: `+1
		-1`,
		Output: `0`,
	},
	tableTest.Test{
		Input: `+3
		+3
		+4
		-2
		-4`,
		Output: `10`,
	},
	tableTest.Test{
		Input: `-6
		+3
		+8
		+5
		-6`,
		Output: `5`,
	},
	tableTest.Test{
		Input: `+7
		+7
		-2
		-7
		-4`,
		Output: `14`,
	},
}

func TestSolve(t *testing.T) {
	tableTest.Run(solve, tests, t)
}
