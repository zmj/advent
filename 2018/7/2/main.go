package main

import (
	"fmt"
	"os"
)

const inputFilename = "input"

func main() {
	f, err := os.Open(inputFilename)
	if err != nil {
		fmt.Printf("Failed to open '%v': %v", inputFilename, err)
		return
	}
	defer f.Close()
	err = solve(f, os.Stdout)
	if err != nil {
		fmt.Printf("Failed: %v", err)
		return
	}
	fmt.Println()
}
