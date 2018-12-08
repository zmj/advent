package main

import (
	"advent/tableTest"
	"testing"
)

var tests = []tableTest.Test{
	tableTest.Test{
		Input:  `2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2`,
		Output: `138`,
	},
}

func TestSolve(t *testing.T) {
	tableTest.Run(solve, tests, t)
}
