package tableTest

import (
	"bytes"
	"io"
	"strconv"
	"strings"
	"testing"
)

// Test defines an expected output given a specified input.
type Test struct {
	Input  string
	Output string
}

// RwFunc consumes input from a Reader and writes output to a Writer.
type RwFunc func(io.Reader, io.Writer) error

// Run executes a set of Tests on an RwFunc.
func Run(f RwFunc, tts []Test, t *testing.T) {
	for i, tt := range tts {
		t.Run(strconv.Itoa(i+1), func(t *testing.T) {
			tt.run(f, t)
		})
	}
}

func (tt Test) run(f RwFunc, t *testing.T) {
	var b bytes.Buffer
	err := f(strings.NewReader(tt.Input), &b)
	if err != nil {
		t.Error(err)
		t.FailNow()
	}
	s := b.String()
	if s != tt.Output {
		t.Errorf("expected '%v' actual '%v'", tt.Output, s)
	}
}
