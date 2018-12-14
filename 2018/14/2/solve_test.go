package main

import (
	"advent/tableTest"
	"testing"
)

var tests = []tableTest.Test{
	tableTest.Test{
		Input:  `51589`,
		Output: `9`,
	},
	tableTest.Test{
		Input:  `01245`,
		Output: `5`,
	},
	tableTest.Test{
		Input:  `92510`,
		Output: `18`,
	},
	tableTest.Test{
		Input:  `59414`,
		Output: `2018`,
	},
}

func TestSolve(t *testing.T) {
	tableTest.Run(solve, tests, t)
}
