package mains;

import toolkitAndUtilities.SCommon;

public class Test20230504 {
	public static void main(String[] args) {
		try {

			// -- choose one --

			test01();

			// --
		}
		catch (Throwable e) {
			e.printStackTrace();
		}
	}

	private static void test01() {
		byte[] data = SCommon.re(() -> "Mercury, Venus, Earth, Mars, Jupiter, Saturn, Uranus, Neptune, Planet-9".getBytes(SCommon.CHARSET_UTF8));
		//byte[] data = SCommon.cryptRandom.getBytes(30);
		//byte[] data = SCommon.cryptRandom.getBytes(100);
		byte[] gzData = SCommon.compress(data);
		String gzStr = SCommon.Base64.encode(gzData);

		System.out.println(SCommon.Hex.toString(data));
		System.out.println(SCommon.Hex.toString(gzData));
		System.out.println(gzStr);

		// ----

		String zs = "H4sIAAAAAAAEAPNNLUouLarUUQhLzSst1lFwTSwqydBR8E0sAnK8SgsyS1KLdBSCE0tKi/J0FEKLEsGq/FILSkrzUnUUAnIS81JLdC0Bufv0hEcAAAA="; // C#
		byte[] zd = SCommon.Base64.decode(zs);
		byte[] dd = SCommon.decompress(zd);
		String ds = SCommon.re(() -> new String(dd, SCommon.CHARSET_UTF8));

		System.out.println(ds);
	}
}
