package main

import (
	"advent/tableTest"
	"testing"
)

var tests = []tableTest.Test{
	tableTest.Test{
		Input: `Step C must be finished before step A can begin.
		Step C must be finished before step F can begin.
		Step A must be finished before step B can begin.
		Step A must be finished before step D can begin.
		Step B must be finished before step E can begin.
		Step D must be finished before step E can begin.
		Step F must be finished before step E can begin.`,
		Output: `CABDFE`,
	},
}

func TestSolve(t *testing.T) {
	tableTest.Run(solve, tests, t)
}
