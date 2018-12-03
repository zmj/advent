package main

import (
	"advent/tableTest"
	"testing"
)

var tests = []tableTest.Test{
	tableTest.Test{
		Input: `#1 @ 1,3: 4x4
		#2 @ 3,1: 4x4
		#3 @ 5,5: 2x2`,
		Output: `4`,
	},
}

func TestSolve(t *testing.T) {
	tableTest.Run(solve, tests, t)
}
