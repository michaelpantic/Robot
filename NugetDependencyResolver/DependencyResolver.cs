using NugetDependencyResolver.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;
using QuickGraph.Graphviz;

namespace NugetDependencyResolver
{
    public class DependencyResolver
    {
        public List<DependencyDto> dependencies = new List<DependencyDto>();

        public BidirectionalGraph<PackageDto, TaggedEdge<PackageDto, EdgeType>> graph = new BidirectionalGraph<PackageDto, TaggedEdge<PackageDto, EdgeType>>(false);

        public DependencyResolver(string[] startPaths)
        {
            foreach (string s in startPaths)
            {
                var solutions = Solution.GetSolutions(s);
                foreach (Solution solution in solutions)
                {

                    dependencies.AddRange(solution.GetDependencies());
                }
            }


          
        }

        public enum EdgeType
        {
            DependsOn,
            Affects
        }


        /// <summary>
        /// Generation 0 = only direct attacheds
        /// Generation 1 = the first circle after the direct attacheds etc, without the inner ones!
        /// </summary>
        /// <param name="generation"></param>
        /// <returns></returns>
        public IEnumerable<PackageDto> GetPackagesToRebuild(PackageDto mainPackage, int generation)
        {
            List<PackageDto> packages = new List<PackageDto>();
            List<PackageDto> tempPackages = new List<PackageDto>();

            packages.Add(mainPackage);
            for (int i = 0; i < generation+1; i++)
            {
                //move one generation further
                tempPackages.Clear();
                tempPackages.AddRange(packages);
                packages.Clear();

                foreach (PackageDto package in tempPackages)
                {
                    foreach (Edge<PackageDto> subpkg in graph.InEdges(package))
                    {
                        packages.Add(subpkg.Source);

                    }
                }
            
            }

            return packages;

        }

        public void BuildTree()
        {
            graph.Clear();
          

            foreach (DependencyDto dto in dependencies)
            {
               /* if (dto.To.Id.Contains("log4net") || dto.To.ThirdParty)
                {
                    continue;
                }*/

                if (!graph.ContainsVertex(dto.FromPkg))
                {
                    graph.AddVertex(dto.FromPkg);
                }

                if (!graph.ContainsVertex(dto.To))
                {
                    graph.AddVertex(dto.To);
                }

                graph.AddEdge(new TaggedEdge<PackageDto, EdgeType>(dto.FromPkg, dto.To, EdgeType.DependsOn));

         

                
                 
                
            }
        
        }

        public void WriteTree()
        {

            BidirectionalGraph<PackageDto, TaggedEdge<PackageDto, EdgeType>> drawgraph =graph.Clone();
            drawgraph.RemoveEdgeIf(x => x.Tag == EdgeType.Affects);
            IVertexAndEdgeListGraph<PackageDto, TaggedEdge<PackageDto, EdgeType>> g = drawgraph;
            
            var graphviz = new GraphvizAlgorithm<PackageDto, TaggedEdge<PackageDto, EdgeType>>(g);
            graphviz.FormatVertex += graphviz_FormatVertex;
            graphviz.GraphFormat.RankDirection = QuickGraph.Graphviz.Dot.GraphvizRankDirection.LR;
            string test = graphviz.Generate();

            System.IO.File.WriteAllText("test.dot", test);
            


        }

        void graphviz_FormatVertex(object sender, FormatVertexEventArgs<PackageDto> e)
        {
          e.VertexFormatter.Label = e.Vertex.ToString();
        }





    }
}
