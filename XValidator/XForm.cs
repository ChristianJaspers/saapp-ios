﻿using System.Collections.Generic;

namespace XValidator
{
    public class XForm<T>
    {
        public XField<T>[] Inputs { get; set; }
        public List<string> Errors { get; set; }
        
        /// <summary>
        /// Validate inputs
        /// </summary>
        public bool Validate()
        {
            CleanErrors();
            
            foreach (var input in Inputs)
            {
                if (!input.Validate())
                {
                    input.ViewStateError();
                }
                
                foreach (var error in input.Errors)
                {
                    Errors.Add(error);
                }
            }
            
            return Errors.Count == 0;
        }
        
        /// <summary>
        /// Cleans the input errors.
        /// </summary>
        void CleanErrors()
        {
            foreach (var input in Inputs)
            {
                input.ViewStateNormal();
                input.Errors = new List<string>();
            }
            
            Errors = new List<string>();
        }
    }
}

