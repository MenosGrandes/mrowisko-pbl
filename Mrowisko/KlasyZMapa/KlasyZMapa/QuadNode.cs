﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map
{
    /// <summary>
    /// Class represent QuadTreeNode. Every node has NEIGHBORS and CHILDREN
    /// </summary>
    public class QuadNode
    {
        bool _isSplit;
        QuadNode _parent;
        QuadTree _parentTree;
        int _positionIndex;

        int _nodeDepth;
        int _nodeSize;

        bool HasChildren;

        #region VERTICES
        public QuadNodeVertex VertexTopLeft;
        public QuadNodeVertex VertexTop;
        public QuadNodeVertex VertexTopRight;
        public QuadNodeVertex VertexLeft;
        public QuadNodeVertex VertexCenter;
        public QuadNodeVertex VertexRight;
        public QuadNodeVertex VertexBottomLeft;
        public QuadNodeVertex VertexBottom;
        public QuadNodeVertex VertexBottomRight;
        #endregion

        #region CHILDREN
        public QuadNode ChildTopLeft;
        public QuadNode ChildTopRight;
        public QuadNode ChildBottomLeft;
        public QuadNode ChildBottomRight;
        #endregion

        #region NEIGHBORS
        public QuadNode NeighborTop;
        public QuadNode NeighborRight;
        public QuadNode NeighborBottom;
        public QuadNode NeighborLeft;
        #endregion

        public BoundingBox Bounds;

        public NodeType NodeType;
        public bool IsSplit { get { return _isSplit; } }

        /// <summary>
        /// Returns true of the node contains enough vertices to split
        /// </summary>
        public bool CanSplit { get { return (_nodeSize >= 2); } }

        ///<summary>
        /// Parent node reference
        ///</summary>
        public QuadNode Parent
        {
            get { return _parent; }
        }
        public QuadNode(NodeType nodeType, int nodeSize, int nodeDepth, QuadNode parent, QuadTree parentTree, int positionIndex)
        {
            NodeType = nodeType;
            _nodeSize = nodeSize;
            _nodeDepth = nodeDepth;
            _positionIndex = positionIndex;

            _parent = parent;
            _parentTree = parentTree;

            //Add the 9 vertices
            AddVertices();

            Bounds = new BoundingBox(_parentTree.Vertices[VertexTopLeft.Index].Position,
                        _parentTree.Vertices[VertexBottomRight.Index].Position);

         

            if (nodeSize >= 4)
                AddChildren();

            //Make call to UpdateNeighbors from the parent node.
            //This will update all neighbors recursively for the
            //children as well.  This ensures all nodes are created
            //prior to updating neighbors.
            if (_nodeDepth == 1)
            {
                AddNeighbors();

                VertexTopLeft.Activated = true;
                VertexTopRight.Activated = true;
                VertexCenter.Activated = true;
                VertexBottomLeft.Activated = true;
                VertexBottomRight.Activated = true;

            }
        }
        private void AddVertices()
        {
            switch (NodeType)
            {
                case NodeType.TopLeft:
                    VertexTopLeft = _parent.VertexTopLeft;
                    VertexTopRight = _parent.VertexTop;
                    VertexBottomLeft = _parent.VertexLeft;
                    VertexBottomRight = _parent.VertexCenter;
                    break;

                case NodeType.TopRight:
                    VertexTopLeft = _parent.VertexTop;
                    VertexTopRight = _parent.VertexTopRight;
                    VertexBottomLeft = _parent.VertexCenter;
                    VertexBottomRight = _parent.VertexRight;
                    break;

                case NodeType.BottomLeft:
                    VertexTopLeft = _parent.VertexLeft;
                    VertexTopRight = _parent.VertexCenter;
                    VertexBottomLeft = _parent.VertexBottomLeft;
                    VertexBottomRight = _parent.VertexBottom;
                    break;

                case NodeType.BottomRight:
                    VertexTopLeft = _parent.VertexCenter;
                    VertexTopRight = _parent.VertexRight;
                    VertexBottomLeft = _parent.VertexBottom;
                    VertexBottomRight = _parent.VertexBottomRight;
                    break;

                default:

                    VertexTopLeft = new QuadNodeVertex { Activated = true, Index = 0 };
                    VertexTopRight = new QuadNodeVertex
                    {
                        Activated = true,
                        Index = VertexTopLeft.Index + _nodeSize
                    };

                    VertexBottomLeft = new QuadNodeVertex
                    {
                        Activated = true,
                        Index = (_parentTree.TopNodeSize + 1) * _parentTree.TopNodeSize
                    };

                    VertexBottomRight = new QuadNodeVertex
                    {
                        Activated = true,
                        Index = VertexBottomLeft.Index + _nodeSize
                    };

                    break;
            }

            VertexTop = new QuadNodeVertex
            {
                Activated = false,
                Index = VertexTopLeft.Index + (_nodeSize / 2)
            };

            VertexLeft = new QuadNodeVertex
            {
                Activated = false,
                Index = VertexTopLeft.Index + (_parentTree.TopNodeSize + 1) * (_nodeSize / 2)
            };

            VertexCenter = new QuadNodeVertex
            {
                Activated = false,
                Index = VertexLeft.Index + (_nodeSize / 2)
            };

            VertexRight = new QuadNodeVertex
            {
                Activated = false,
                Index = VertexLeft.Index + _nodeSize
            };

            VertexBottom = new QuadNodeVertex
            {
                Activated = false,
                Index = VertexBottomLeft.Index + (_nodeSize / 2)
            };
        }

        /// <summary>
        /// Add the 4 child quad nodes.
        /// </summary>
        private void AddChildren()
        {
            //Add top left (northwest) child
            ChildTopLeft = new QuadNode(NodeType.TopLeft, _nodeSize / 2, _nodeDepth + 1, this, _parentTree, VertexTopLeft.Index);

            //Add top right (northeast) child
            ChildTopRight = new QuadNode(NodeType.TopRight, _nodeSize / 2, _nodeDepth + 1, this, _parentTree, VertexTop.Index);

            //Add bottom left (southwest) child
            ChildBottomLeft = new QuadNode(NodeType.BottomLeft, _nodeSize / 2, _nodeDepth + 1, this, _parentTree, VertexLeft.Index);

            //Add bottom right (southeast) child
            ChildBottomRight = new QuadNode(NodeType.BottomRight, _nodeSize / 2, _nodeDepth + 1, this, _parentTree, VertexCenter.Index);

            HasChildren = true;

        }

        /// <summary>
        /// Update reference to neighboring quads
        /// </summary>
        private void AddNeighbors()
        {
            switch (NodeType)
            {
                case NodeType.TopLeft: //Top Left Corner
                    //Top neighbor
                    if (_parent.NeighborTop != null)
                        NeighborTop = _parent.NeighborTop.ChildBottomLeft;

                    //Right neighbor
                    NeighborRight = _parent.ChildTopRight;
                    //Bottom neighbor
                    NeighborBottom = _parent.ChildBottomLeft;

                    //Left neighbor
                    if (_parent.NeighborLeft != null)
                        NeighborLeft = _parent.NeighborLeft.ChildTopRight;

                    break;

                case NodeType.TopRight: //Top Right Corner
                    //Top neighbor
                    if (_parent.NeighborTop != null)
                        NeighborTop = _parent.NeighborTop.ChildBottomRight;

                    //Right neighbor
                    if (_parent.NeighborRight != null)
                        NeighborRight = _parent.NeighborRight.ChildTopLeft;

                    //Bottom neighbor
                    NeighborBottom = _parent.ChildBottomRight;

                    //Left neighbor
                    NeighborLeft = _parent.ChildTopLeft;

                    break;

                case NodeType.BottomLeft: //Bottom Left Corner
                    //Top neighbor
                    NeighborTop = _parent.ChildTopLeft;

                    //Right neighbor
                    NeighborRight = _parent.ChildBottomRight;

                    //Bottom neighbor
                    if (_parent.NeighborBottom != null)
                        NeighborBottom = _parent.NeighborBottom.ChildTopLeft;

                    //Left neighbor
                    if (_parent.NeighborLeft != null)
                        NeighborLeft = _parent.NeighborLeft.ChildBottomRight;

                    break;

                case NodeType.BottomRight: //Bottom Right Corner
                    //Top neighbor
                    NeighborTop = _parent.ChildTopRight;

                    //Right neighbor
                    if (_parent.NeighborRight != null)
                        NeighborRight = _parent.NeighborRight.ChildBottomLeft;

                    //Bottom neighbor
                    if (_parent.NeighborBottom != null)
                        NeighborBottom = _parent.NeighborBottom.ChildTopRight;

                    //Left neighbor
                    NeighborLeft = _parent.ChildBottomLeft;

                    break;
            }


            if (this.HasChildren)
            {
                ChildTopLeft.AddNeighbors();
                ChildTopRight.AddNeighbors();
                ChildBottomLeft.AddNeighbors();
                ChildBottomRight.AddNeighbors();
            }

        }
        internal void SetActiveVertices()
        {
           
            if (_parentTree.Cull && !IsInView)
                return;
            if (_isSplit && this.HasChildren)
            {
                ChildTopLeft.SetActiveVertices();
                ChildTopRight.SetActiveVertices();
                ChildBottomLeft.SetActiveVertices();
                ChildBottomRight.SetActiveVertices();
                return;
            }
            //Top Triangle(s)
            _parentTree.UpdateBuffer(VertexCenter.Index);
            _parentTree.UpdateBuffer(VertexTopLeft.Index);

            if (VertexTop.Activated)
            {
                _parentTree.UpdateBuffer(VertexTop.Index);

                _parentTree.UpdateBuffer(VertexCenter.Index);
                _parentTree.UpdateBuffer(VertexTop.Index);
            }
            _parentTree.UpdateBuffer(VertexTopRight.Index);

            //Right Triangle(s)
            _parentTree.UpdateBuffer(VertexCenter.Index);
            _parentTree.UpdateBuffer(VertexTopRight.Index);

            if (VertexRight.Activated)
            {
                _parentTree.UpdateBuffer(VertexRight.Index);

                _parentTree.UpdateBuffer(VertexCenter.Index);
                _parentTree.UpdateBuffer(VertexRight.Index);
            }
            _parentTree.UpdateBuffer(VertexBottomRight.Index);

            //Bottom Triangle(s)
            _parentTree.UpdateBuffer(VertexCenter.Index);
            _parentTree.UpdateBuffer(VertexBottomRight.Index);

            if (VertexBottom.Activated)
            {
                _parentTree.UpdateBuffer(VertexBottom.Index);

                _parentTree.UpdateBuffer(VertexCenter.Index);
                _parentTree.UpdateBuffer(VertexBottom.Index);
            }
            _parentTree.UpdateBuffer(VertexBottomLeft.Index);

            //Left Triangle(s)
            _parentTree.UpdateBuffer(VertexCenter.Index);
            _parentTree.UpdateBuffer(VertexBottomLeft.Index);

            if (VertexLeft.Activated)
            {
                _parentTree.UpdateBuffer(VertexLeft.Index);

                _parentTree.UpdateBuffer(VertexCenter.Index);
                _parentTree.UpdateBuffer(VertexLeft.Index);
            }
            _parentTree.UpdateBuffer(VertexTopLeft.Index);
        }
        internal void Activate()
        {
            VertexTopLeft.Activated = true;
            VertexTopRight.Activated = true;
            VertexCenter.Activated = true;
            VertexBottomLeft.Activated = true;
            VertexBottomRight.Activated = true;

        }
        public void EnforceMinimumDepth()
        {
            if (_nodeDepth < _parentTree.MinimumDepth)
            {
                if (this.HasChildren)
                {
                    _isSplit = true;

                    ChildTopLeft.EnforceMinimumDepth();
                    ChildTopRight.EnforceMinimumDepth();
                    ChildBottomLeft.EnforceMinimumDepth();
                    ChildBottomRight.EnforceMinimumDepth();
                }
                if(this.HasChildren==false)
                {
                    Console.WriteLine(Bounds.Min);

                    _isSplit = false;
                }

                return;
            }

            if (_nodeDepth == _parentTree.MinimumDepth || (_nodeDepth < _parentTree.MinimumDepth && !this.HasChildren))
            {
                this.Activate();
                _isSplit = false;
            }
        }
        public bool Contains(BoundingFrustum boundingFrustrum)
        {
            return Bounds.Intersects(boundingFrustrum);
        }
        public bool IsInView
        {
            get { return Contains(_parentTree.ViewFrustrum); }
        }
        public override string ToString()
        {
            return Bounds.ToString();
        }
    }
}
