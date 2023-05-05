package toolkitAndUtilities;

import java.util.List;
import java.util.Random;

public class RandomUnit {
	private Random _r;

	public RandomUnit(Random r) {
		_r = r;
	}

	public void read(byte[] buff) {
		_r.nextBytes(buff);
	}

	public byte[] getBytes(int size) {
		byte[] buff = new byte[size];
		read(buff);
		return buff;
	}

	public byte getByte() {
		return getBytes(1)[0];
	}

	public int getInt32() {
		byte[] data = getBytes(4);
		int returnValue =
				((data[0] & 0xff) <<  0) |
				((data[1] & 0xff) <<  8) |
				((data[2] & 0xff) << 16) |
				((data[3] & 0xff) << 24);
		return returnValue;
	}

	public int getPositiveInt() {
		return getInt32() & 0x7fffffff;
	}

	public long getLong48() {
		byte[] data = getBytes(6);
		long returnValue =
				((data[0] & 0xffL) <<  0) |
				((data[1] & 0xffL) <<  8) |
				((data[2] & 0xffL) << 16) |
				((data[3] & 0xffL) << 24) |
				((data[4] & 0xffL) << 32) |
				((data[5] & 0xffL) << 40);
		return returnValue;
	}

	public long getLong64() {
		byte[] data = getBytes(8);
		long returnValue =
				((data[0] & 0xffL) <<  0) |
				((data[1] & 0xffL) <<  8) |
				((data[2] & 0xffL) << 16) |
				((data[3] & 0xffL) << 24) |
				((data[4] & 0xffL) << 32) |
				((data[5] & 0xffL) << 40) |
				((data[6] & 0xffL) << 48) |
				((data[7] & 0xffL) << 56);
		return returnValue;
	}

	public long getPositiveLong() {
		return getLong64() & 0x7fffffffffffffffL;
	}

	public long getLong(long modulo) {
		if (modulo < 1) {
			throw new Error("Bad modulo");
		}
		return getPositiveLong() % modulo;
	}

	public int getInt(int modulo) {
		if (modulo < 1) {
			throw new Error("Bad modulo");
		}
		return (int)(getPositiveLong() % (long)modulo);
	}

	public int getRange(int minval, int maxval) {
		return getInt(maxval - minval + 1) + minval;
	}

	public boolean getBoolean() {
		return getInt(2) == 1;
	}

	public int getSign() {
		return getInt(2) * 2 - 1;
	}

	public double getRate() {
		return (double)getLong48() / ((1L << 48) - 1);
	}

	public double getRealRange(double minval, double maxval) {
		return getRate() * (maxval - minval) + minval;
	}

	public <T> T chooseOne(List<T> list) {
		return list.get(getInt(list.size()));
	}

	public <T> void shuffle(List<T> list) {
		for (int index = list.size(); 1 < index; index--) {
			SCommon.swap(list, getInt(index), index - 1);
		}
	}
}
