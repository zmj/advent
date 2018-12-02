package main

import (
	"advent/2018/tableTest"
	"testing"
)

var tests = []tableTest.Test{
	tableTest.Test{
		Input: `abcde
		fghij
		klmno
		pqrst
		fguij
		axcye
		wvxyz`,
		Output: `fgij`,
	},
	tableTest.Test{
		Input: `ab
		ac`,
		Output: `a`,
	},
}

func TestSolve(t *testing.T) {
	tableTest.Run(solve, tests, t)
}
