package main

import (
	"advent/tableTest"
	"testing"
)

var tests = []tableTest.Test{
	tableTest.Test{
		Input:  `dabAcCaCBAcCcaDA`,
		Output: `10`,
	},
}

func TestSolve(t *testing.T) {
	tableTest.Run(solve, tests, t)
}

func TestReactTrue(t *testing.T) {
	ok := react('a', 'A')
	if !ok {
		t.Fail()
	}
}

func TestReactLower(t *testing.T) {
	ok := react('a', 'a')
	if ok {
		t.Fail()
	}
}

func TestReactUpper(t *testing.T) {
	ok := react('A', 'A')
	if ok {
		t.Fail()
	}
}

func TestReactDifferent(t *testing.T) {
	ok := react('a', 'B')
	if ok {
		t.Fail()
	}
}
