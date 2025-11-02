using Android.App;
using Android.Runtime;

namespace ChecklistApp;

[Application]
public class MainApplication : MauiApplication
{
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
	}

	protected override MauiApp CreateMauiApp()
	{
		Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("RemoveUnderLine", (r, v) =>
		{
			r.PlatformView.BackgroundTintList =
				Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.ParseColor("#424A4E"));
		});
		Microsoft.Maui.Handlers.DatePickerHandler.Mapper.AppendToMapping("RemoveUnderLine", (r, v) =>
		{
			r.PlatformView.BackgroundTintList =
				Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.ParseColor("#424A4E"));
		});
		Microsoft.Maui.Handlers.TimePickerHandler.Mapper.AppendToMapping("RemoveUnderLine", (r, v) =>
		{
			r.PlatformView.BackgroundTintList = 
				Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.ParseColor("#424A4E"));
		});

		return MauiProgram.CreateMauiApp();
	}
}
