package main

import (
	"advent/2018/tableTest"
	"testing"
)

var tests = []tableTest.Test{
	tableTest.Test{
		Input: `abcdef
		bababc
		abbcde
		abcccd
		aabcdd
		abcdee
		ababab`,
		Output: `12`,
	},
}

func TestSolve(t *testing.T) {
	tableTest.Run(solve, tests, t)
}
