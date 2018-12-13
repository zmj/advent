package main

import (
	"advent/tableTest"
	"testing"
)

var tests = []tableTest.Test{
	tableTest.Test{
		Input: `
/>-<\  
|   |  
| /<+-\
| | | v
\>+</ |
  |   ^
  \<->/`,
		Output: `6,4`,
	},
}

func TestSolve(t *testing.T) {
	tableTest.Run(solve, tests, t)
}
