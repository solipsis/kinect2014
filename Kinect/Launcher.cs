using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace Kinect {

	class Launcher {

		private String GamePath;

		public Launcher(String path) {
			// IDK, some stuff might go here.

			GamePath = "GameDir\\" + path;

			prepLaunch();
		}

		private void prepLaunch() {
			ProcessStartInfo start = new ProcessStartInfo();

			start.FileName = GamePath;

			start.WindowStyle = ProcessWindowStyle.Normal;

			launch(start);
		}

		private void launch(ProcessStartInfo start) {
			using (Process proc = Process.Start(start)) {
				proc.WaitForExit();

				// Maybe check error codes here? I think grabbing proc.ExitCode might give us some useful info.

				int exitCode = proc.ExitCode;
			}
		}

	}
}
