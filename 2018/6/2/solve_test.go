package main

import (
	"advent/tableTest"
	"testing"
)

var tests = []tableTest.Test{
	tableTest.Test{
		Input: `1, 1
		1, 6
		8, 3
		3, 4
		5, 5
		8, 9`,
		Output: `16`,
	},
}

func TestSolve(t *testing.T) {
	tableTest.Run(solve, tests, t)
}
