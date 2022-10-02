using System.Collections.Generic;
using System.Threading;

namespace RyosMKFXPanel {
	public enum SliderType :byte{
		delay,
		speed
	}

	class LightningModule :ISettings {
		public virtual string GetUid() => this.GetHashCode().ToString();
		public virtual string GetName() => "Unnamed." + this.GetUid();
		public virtual string GetCategory() => "Custom";
		public virtual string GetIcon() => "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAACXBIWXMAAAsTAAALEwEAmpwYAAAGRmlUWHRYTUw6Y29tLmFkb2JlLnhtcAAAAAAAPD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4gPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgNS42LWMxNDggNzkuMTY0MDM2LCAyMDE5LzA4LzEzLTAxOjA2OjU3ICAgICAgICAiPiA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPiA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIiB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iIHhtbG5zOnhtcE1NPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvbW0vIiB4bWxuczpzdEV2dD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL3NUeXBlL1Jlc291cmNlRXZlbnQjIiB4bWxuczpwaG90b3Nob3A9Imh0dHA6Ly9ucy5hZG9iZS5jb20vcGhvdG9zaG9wLzEuMC8iIHhtbG5zOmRjPSJodHRwOi8vcHVybC5vcmcvZGMvZWxlbWVudHMvMS4xLyIgeG1wOkNyZWF0b3JUb29sPSJBZG9iZSBQaG90b3Nob3AgMjEuMSAoV2luZG93cykiIHhtcDpDcmVhdGVEYXRlPSIyMDIxLTA2LTAyVDExOjM5OjM4KzAyOjAwIiB4bXA6TWV0YWRhdGFEYXRlPSIyMDIxLTA2LTAyVDExOjM5OjM4KzAyOjAwIiB4bXA6TW9kaWZ5RGF0ZT0iMjAyMS0wNi0wMlQxMTozOTozOCswMjowMCIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDpiYmZkNTQ2YS1iZDdkLTA3NGMtODlkMC01ZTU2Y2I3NTNiNzgiIHhtcE1NOkRvY3VtZW50SUQ9ImFkb2JlOmRvY2lkOnBob3Rvc2hvcDo1MDAzNTRiMS0wMjE3LTIzNDItOWE5Zi02N2Q0NTBkMThiNWUiIHhtcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD0ieG1wLmRpZDo3ZWQyYTE0OC1lNTlkLTAwNDItYThjMi0wMTlkNzZiNmI3ZjkiIHBob3Rvc2hvcDpDb2xvck1vZGU9IjMiIGRjOmZvcm1hdD0iaW1hZ2UvcG5nIj4gPHhtcE1NOkhpc3Rvcnk+IDxyZGY6U2VxPiA8cmRmOmxpIHN0RXZ0OmFjdGlvbj0iY3JlYXRlZCIgc3RFdnQ6aW5zdGFuY2VJRD0ieG1wLmlpZDo3ZWQyYTE0OC1lNTlkLTAwNDItYThjMi0wMTlkNzZiNmI3ZjkiIHN0RXZ0OndoZW49IjIwMjEtMDYtMDJUMTE6Mzk6MzgrMDI6MDAiIHN0RXZ0OnNvZnR3YXJlQWdlbnQ9IkFkb2JlIFBob3Rvc2hvcCAyMS4xIChXaW5kb3dzKSIvPiA8cmRmOmxpIHN0RXZ0OmFjdGlvbj0ic2F2ZWQiIHN0RXZ0Omluc3RhbmNlSUQ9InhtcC5paWQ6YmJmZDU0NmEtYmQ3ZC0wNzRjLTg5ZDAtNWU1NmNiNzUzYjc4IiBzdEV2dDp3aGVuPSIyMDIxLTA2LTAyVDExOjM5OjM4KzAyOjAwIiBzdEV2dDpzb2Z0d2FyZUFnZW50PSJBZG9iZSBQaG90b3Nob3AgMjEuMSAoV2luZG93cykiIHN0RXZ0OmNoYW5nZWQ9Ii8iLz4gPC9yZGY6U2VxPiA8L3htcE1NOkhpc3Rvcnk+IDxwaG90b3Nob3A6VGV4dExheWVycz4gPHJkZjpCYWc+IDxyZGY6bGkgcGhvdG9zaG9wOkxheWVyTmFtZT0iPyIgcGhvdG9zaG9wOkxheWVyVGV4dD0iPyIvPiA8L3JkZjpCYWc+IDwvcGhvdG9zaG9wOlRleHRMYXllcnM+IDwvcmRmOkRlc2NyaXB0aW9uPiA8L3JkZjpSREY+IDwveDp4bXBtZXRhPiA8P3hwYWNrZXQgZW5kPSJyIj8+95GeYwAAAURJREFUWIXtldGNgzAQRHevg1wJboEWaCEtpAVaoIW0kBYuJSQlXAuXEl4+ApIPdg0mG+UnIyEkI3uezXpWAcmlqmIJ2InIQUTS8J7qPDxXVT07a9iD+WN8T8CJOl2A/ZIXUAYA9pXGU/WbAQLMR52qAYAmyHxUVwvwU1jsMi44qZN+ASKtAqC8+6NV3RlIA/w5c/u1AJ2385J5BuHOXwvgXbnZtXIAkjMfC0AxgghoRKQdhloRaVT1eyVAIyLeac1TbnYkT6r2F3w97fjfPIlI53w24znsBHjcgN/C/193DTeat/jXD2qDqNL8UDCGLDvCAfALbmYeDlCz83AAymEDk17xCoBjwdxNzEgAr+KLcR0CgN8ti53SA9iShDtn3E66BUVG8e3dAJtktuNXyaqxt59ANQCPxmOpXZ4dABCtD8AH4A4WX23XqkOXEwAAAABJRU5ErkJggg==";
		public virtual SliderType GetSliderType() => SliderType.delay;
		public virtual double GetSliderMax() => 3.0d;
		public virtual double GetSliderMin() => 1.0d;
		public virtual double GetSliderTick() => 1.0d;
		public virtual double GetSliderTickFrequency() => 1.0d;
		public virtual LightningModuleControls[] GetControls() => new LightningModuleControls[] { };
		public virtual Dictionary<string, string> GetSettings() => new Dictionary<string, string> {};
		public virtual void LoadSettings(in Dictionary<string, string> settings) {}
		public LightningModule() { }

		public bool running;
		//public int deviceID;

		private Thread _thread;
		public virtual void Start() {
			_thread = new Thread(Work);
			_thread.Start();
			running = true;
		}
		public virtual void Stop() {
			_thread.Abort();
			running = false;
		}

		public void Restart() {
			Start();
			Stop();
		}

		public virtual void Work() { }

	}
}
