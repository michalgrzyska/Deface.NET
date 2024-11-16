﻿namespace Deface.NET.UnitTests;

public class PublicMembersTests
{
    [Fact]
    public void AreAllPublicMembersInCorrectNamespace()
    {
        var assembly = typeof(DefaceService).Assembly;
        var publicTypes = assembly.GetExportedTypes();

        publicTypes.Should().OnlyContain(x => x.Namespace == "Deface.NET");
    }
}
