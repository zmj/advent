package main

import (
	"advent/tableTest"
	"testing"
)

var tests = []tableTest.Test{
	tableTest.Test{
		Input:  ``,
		Output: `90,269,16`,
	},
}

func TestSolve(t *testing.T) {
	tableTest.Run(solve, tests, t)
}

func TestPower1(t *testing.T) {
	if power(3, 5, 8) != 4 {
		t.Fail()
	}
}

func TestPower2(t *testing.T) {
	if power(122, 79, 57) != -5 {
		t.Fail()
	}
}

func TestPower3(t *testing.T) {
	if power(217, 196, 39) != 0 {
		t.Fail()
	}
}

func TestPower4(t *testing.T) {
	if power(101, 153, 71) != 4 {
		t.Fail()
	}
}
