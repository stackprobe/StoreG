package toolkitAndUtilities;

import java.util.Comparator;

public interface IComparator<T> {
	public int run(T a, T b);

	public default Comparator<T> toComparator() {
		return (a, b) -> this.run(a, b);
	}
}
