using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Unmockable.Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UnmockableAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "UnmockableAnalyzer";

        private static readonly string Title = "Only use Unmockable for non-virtual types";
        private static readonly string MessageFormat = "Member '{0}' is virtual";
        private static readonly string Description = "Use regular mocking frameworks for virtual methods and interfaces.";
        private const string Category = "Usage";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.InvocationExpression);
        }

        private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            var expr = (InvocationExpressionSyntax)context.Node;

            if (context.SemanticModel.GetSymbolInfo(expr.Expression).CandidateSymbols.Any(x => x.ContainingType.Name != "IUnmockable"))
                return;

            var lambda = (SimpleLambdaExpressionSyntax)expr.ArgumentList.Arguments.First().Expression;
            var other = (InvocationExpressionSyntax)lambda.Body;
            var symbols = context.SemanticModel.GetSymbolInfo(other).CandidateSymbols;

            if (!symbols.Any())
            {
                throw new Exception("no symbols found");
            }
        }
    }
}
