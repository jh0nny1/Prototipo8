using System;
using System.IO;
using System.Text;
using Gamelogic;
using UnityEngine;

namespace Memoria
{
	public class CsvCreator
	{
		private readonly string _filePath;
		private readonly int _actualPersonId;

		private const string PersonId = "PersonId";

		public CsvCreator(string filePath)
		{
			_filePath = filePath;

            _actualPersonId = GLPlayerPrefs.GetInt(ProfileManager.Instance.currentEvaluationScope, "UserID");

            try
            {
                AddLines("UserID", "-");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            
		}

        /// <summary>
        /// Adds a new line to the csv in the _filePath, defined on creation
        /// </summary>
        /// <param name="action"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
		public bool AddLines(string action, string objectId)
		{
			var csv = new StringBuilder();

			var actualHour = DateTime.Now.TimeOfDay;
			var actualTimestamp = DateTime.Now.Date.ToShortDateString();
			var newLine = string.Format("{0},{1},{2},{3},{4}", _actualPersonId, actualHour, actualTimestamp, action, objectId);
			csv.AppendLine(newLine);

            try {
                File.AppendAllText(_filePath, csv.ToString());
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }
			

			return true;
		}

        /// <summary>
        /// Adds line to specified subfolder file under _filePath
        /// </summary>
        /// <param name="action"></param>
        /// <param name="objectId"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool AddLines(string action, string objectId, string subfolderFile)
        {
            var csv = new StringBuilder();

            var actualHour = DateTime.Now.TimeOfDay;
            var actualTimestamp = DateTime.Now.Date.ToShortDateString();
            var newLine = string.Format("{0},{1},{2},{3},{4}", _actualPersonId, actualHour, actualTimestamp, action, objectId);
            csv.AppendLine(newLine);
            string auxPath = _filePath + subfolderFile;
            try
            {
                File.AppendAllText(auxPath, csv.ToString());
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            return true;
        }
	}
}