using Should.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Should.Facts
{
    internal class SampleCodeForReadmeFile
    {
        public void Should_assertions()
        {
            object obj = null;
            obj.ShouldBeNull();

            obj = new object();
            obj.ShouldBeType(typeof(object));
            obj.ShouldEqual(obj);
            obj.ShouldNotBeNull();
            obj.ShouldNotBeSameAs(new object());
            obj.ShouldNotBeType(typeof(string));
            obj.ShouldNotEqual("foo");

            obj = "x";
            obj.ShouldNotBeInRange("y", "z");
            obj.ShouldBeInRange("a", "z");
            obj.ShouldBeSameAs("x");

            "This String".ShouldContain("This");
            "This String".ShouldNotBeEmpty();
            "This String".ShouldNotContain("foobar");

            false.ShouldBeFalse();
            true.ShouldBeTrue();

            var list = new List<object>();
            list.ShouldBeEmpty();
            list.ShouldNotContain(new object());

            var item = new object();
            list.Add(item);
            list.ShouldNotBeEmpty();
            list.ShouldContain(item);
        }

        public void Should_fluent_assertions()
        {
            object obj = null;
            obj.Should().Be.Null();

            obj = new object();
            obj.Should().Be.OfType(typeof(object));
            obj.Should().Equal(obj);
            obj.Should().Not.Be.Null();
            obj.Should().Not.Be.SameAs(new object());
            obj.Should().Not.Be.OfType<string>();
            obj.Should().Not.Equal("foo");

            obj = "x";
            obj.Should().Not.Be.InRange("y", "z");
            obj.Should().Be.InRange("a", "z");
            obj.Should().Be.SameAs("x");

            "This String".Should().Contain("This");
            "This String".Should().Not.Be.Empty();
            "This String".Should().Not.Contain("foobar");

            false.Should().Be.False();
            true.Should().Be.True();

            var list = new List<object>();
            list.Should().Count.Zero();
            list.Should().Not.Contain.Item(new object());

            var item = new object();
            list.Add(item);
            list.Should().Not.Be.Empty();
            list.Should().Contain.Item(item);
        }

        public void Should_fluent_assertions_additional_samples()
        {
            var numbers = new List<int> { 1, 1, 2, 3 };
            numbers.Should().Contain.Any(x => x == 1);
            numbers
                .Should().Count.AtLeast(1)
                .Should().Count.NoMoreThan(5)
                .Should().Count.Exactly(4)
                .Should().Contain.One(x => x > 2);

            var id = new Guid();
            id.Should().Be.Empty();

            id = Guid.NewGuid();
            id.Should().Not.Be.Empty();

            var date = DateTime.Now;
            date.Should().Be.Today();

            var str = "";
            str.Should().Be.NullOrEmpty();

            var one = "1";
            one.Should().Be.ConvertableTo<int>();

            var idString = Guid.NewGuid().ToString();
            idString.Should().Be.ConvertableTo<Guid>();
        }
    }
}
