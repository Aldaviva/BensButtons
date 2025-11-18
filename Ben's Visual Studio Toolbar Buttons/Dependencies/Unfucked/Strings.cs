#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Aldaviva.VisualStudioToolbarButtons.Dependencies.Unfucked;

/*
 * This file is copied from Aldaviva/Unfucked because NuGet package dependencies can't be loaded in VSIX Visual Studio Extensions.
 * It fails with a FileNotFoundException, and if you try to add an assembly:ProvidedCodeBase to specify where to find the dependency's DLL file, it fails with the error "The private assembly was located outside the appbase directory".
 */
internal static class Strings {

    /// <summary>
    /// Remove all occurrences of any of a given array of prefixes from the beginning of a string.
    /// </summary>
    /// <param name="str">The text to remove prefixes from.</param>
    /// <param name="prefixesToTrim">Zero or more prefixes, all of which will be removed from <paramref name="str"/>.</param>
    /// <returns>A substring of <paramref name="str"/> that does not start with any of the <paramref name="prefixesToTrim"/>.</returns>
    [Pure]
    public static string TrimStart(this string str, params string[] prefixesToTrim) => TrimStart(str.AsSpan(), -1, prefixesToTrim);

    /// <inheritdoc cref="TrimStart(string,string[])" />
    [Pure]
    public static string TrimStart(this string str, params IEnumerable<string> prefixesToTrim) => TrimStart(str.AsSpan(), -1, prefixesToTrim.ToArray());

    /// <inheritdoc cref="TrimStart(ReadOnlySpan{char},int,string[])" />
    [Pure]
    public static string TrimStart(this string str, int limit = -1, params string[] prefixesToTrim) => TrimStart(str.AsSpan(), limit, prefixesToTrim);

    /// <inheritdoc cref="TrimStart(ReadOnlySpan{char},int,string[])" />
    [Pure]
    public static string TrimStart(this string str, int limit = -1, params IEnumerable<string> prefixesToTrim) => TrimStart(str.AsSpan(), limit, prefixesToTrim.ToArray());

    /// <inheritdoc cref="TrimStart(ReadOnlySpan{char},int,string[])" />
    [Pure]
    public static string TrimStart(this string str, int limit = -1, params char[] prefixesToTrim) => TrimStart(str.AsSpan(), limit, prefixesToTrim.Select(c => c.ToString()).ToArray());

    /// <inheritdoc cref="TrimStart(ReadOnlySpan{char},int,string[])" />
    [Pure]
    public static string TrimStart(this string str, int limit = -1, params IEnumerable<char> prefixesToTrim) => TrimStart(str.AsSpan(), limit, prefixesToTrim.Select(c => c.ToString()).ToArray());

    /// <inheritdoc cref="TrimStart(string,IEnumerable{string})" />
    [Pure]
    public static string TrimStart(this ReadOnlySpan<char> str, params string[] prefixesToTrim) => TrimStart(str, -1, prefixesToTrim);

    /// <inheritdoc cref="TrimStart(string,IEnumerable{string})" />
    [Pure]
    public static string TrimStart(this ReadOnlySpan<char> str, params IEnumerable<string> prefixesToTrim) => TrimStart(str, -1, prefixesToTrim.ToArray());

    /// <inheritdoc cref="TrimStart(ReadOnlySpan{char},int,string[])" />
    [Pure]
    public static string TrimStart(this ReadOnlySpan<char> str, int limit = -1, params IEnumerable<string> prefixesToTrim) => TrimStart(str, limit, prefixesToTrim.ToArray());

    /// <summary>
    /// Remove some occurrences of any of a given array of prefixes from the beginning of a string.
    /// </summary>
    /// <param name="str">The text to remove prefixes from.</param>
    /// <param name="limit">The maximum number of occurrences to remove, or <c>-1</c> to remove all occurrences.</param>
    /// <param name="prefixesToTrim">Zero or more prefixes, all of which will be removed from <paramref name="str"/>.</param>
    /// <returns>A substring of <paramref name="str"/> that does not start with any of the <paramref name="prefixesToTrim"/>.</returns>
    [Pure]
    public static string TrimStart(this ReadOnlySpan<char> str, int limit = -1, params string[] prefixesToTrim) {
        int  startIndex  = 0;
        uint occurrences = 0;
        while (limit == -1 || occurrences < limit) {
            bool found = false;
            foreach (string prefixToTrim in prefixesToTrim) {
                int prefixLength = prefixToTrim.Length;
                if (prefixLength != 0 && startIndex + prefixLength <= str.Length && str.Slice(startIndex, prefixLength).SequenceEqual(prefixToTrim.AsSpan())) {
                    startIndex += prefixLength;
                    found      =  true;
                    occurrences++;
                    break;
                }
            }

            if (!found) {
                break;
            }
        }

        return str.Slice(startIndex).ToString();
    }

    /// <summary>
    /// Remove all occurrences of any of a given array of suffixes from the end of a string.
    /// </summary>
    /// <param name="str">The text to remove suffixes from.</param>
    /// <param name="suffixesToTrim">Zero or more suffixes, all of which will be removed from <paramref name="str"/>.</param>
    /// <returns>A substring of <paramref name="str"/> that does not end with any of the <paramref name="suffixesToTrim"/>.</returns>
    [Pure]
    public static string TrimEnd(this string str, params string[] suffixesToTrim) => TrimEnd(str.AsSpan(), -1, suffixesToTrim);

    /// <inheritdoc cref="TrimEnd(string,string[])" />
    [Pure]
    public static string TrimEnd(this string str, params IEnumerable<string> suffixesToTrim) => TrimEnd(str.AsSpan(), -1, suffixesToTrim.ToArray());

    /// <inheritdoc cref="TrimEnd(string,string[])" />
    [Pure]
    public static string TrimEnd(this string str, params char[] suffixesToTrim) => TrimEnd(str.AsSpan(), -1, suffixesToTrim.Select(c => c.ToString()));

    /// <inheritdoc cref="TrimEnd(string,string[])" />
    [Pure]
    public static string TrimEnd(this string str, params IEnumerable<char> suffixesToTrim) => TrimEnd(str.AsSpan(), -1, suffixesToTrim.Select(c => c.ToString()).ToArray());

    /// <inheritdoc cref="TrimEnd(string,int,string[])" />
    [Pure]
    public static string TrimEnd(this string str, int limit = -1, params string[] suffixesToTrim) => TrimEnd(str.AsSpan(), limit, suffixesToTrim);

    /// <inheritdoc cref="TrimEnd(string,int,string[])" />
    [Pure]
    public static string TrimEnd(this string str, int limit = -1, params IEnumerable<string> suffixesToTrim) => TrimEnd(str.AsSpan(), limit, suffixesToTrim.ToArray());

    /// <inheritdoc cref="TrimEnd(string,string[])" />
    [Pure]
    public static string TrimEnd(this ReadOnlySpan<char> str, params string[] suffixesToTrim) => TrimEnd(str, -1, suffixesToTrim);

    /// <inheritdoc cref="TrimEnd(string,string[])" />
    [Pure]
    public static string TrimEnd(this ReadOnlySpan<char> str, params IEnumerable<string> suffixesToTrim) => TrimEnd(str, -1, suffixesToTrim.ToArray());

    /// <inheritdoc cref="TrimEnd(string,int,string[])" />
    [Pure]
    public static string TrimEnd(this ReadOnlySpan<char> str, int limit = -1, params IEnumerable<string> suffixesToTrim) => TrimEnd(str, limit, suffixesToTrim.ToArray());

    /// <summary>
    /// Remove some occurrences of any of a given array of suffixes from the end of a string.
    /// </summary>
    /// <param name="str">The text to remove suffixes from.</param>
    /// <param name="limit">The maximum number of occurrences to remove, or <c>-1</c> to remove all occurrences.</param>
    /// <param name="suffixesToTrim">Zero or more suffixes, all of which will be removed from <paramref name="str"/>.</param>
    /// <returns>A substring of <paramref name="str"/> that does not end with any of the <paramref name="suffixesToTrim"/>.</returns>
    [Pure]
    public static string TrimEnd(this ReadOnlySpan<char> str, int limit = -1, params string[] suffixesToTrim) {
        int  endIndex    = str.Length;
        uint occurrences = 0;
        while (limit == -1 || occurrences < limit) {
            bool found = false;
            foreach (string suffixToTrim in suffixesToTrim) {
                int suffixLength = suffixToTrim.Length;
                if (suffixLength != 0 && endIndex >= 0 && endIndex - suffixLength >= 0 && str.Slice(endIndex - suffixLength, suffixLength).SequenceEqual(suffixToTrim.AsSpan())) {
                    endIndex -= suffixLength;
                    found    =  true;
                    occurrences++;
                    break;
                }
            }

            if (!found) {
                break;
            }
        }

        return str.Slice(0, endIndex).ToString();
    }

    /// <inheritdoc cref="Trim(ReadOnlySpan{char},string[])" />
    [Pure]
    public static string Trim(this string str, params string[] affixesToTrim) => Trim(str.AsSpan(), affixesToTrim);

    /// <inheritdoc cref="Trim(ReadOnlySpan{char},string[])" />
    [Pure]
    public static string Trim(this string str, params IEnumerable<string> affixesToTrim) => Trim(str.AsSpan(), affixesToTrim.ToArray());

    /// <inheritdoc cref="Trim(ReadOnlySpan{char},string[])" />
    [Pure]
    public static string Trim(this ReadOnlySpan<char> str, params IEnumerable<string> affixesToTrim) => Trim(str, affixesToTrim.ToArray());

    /// <summary>
    /// Remove all occurrences of any of a given array of affixes from the beginning and end of a string.
    /// </summary>
    /// <param name="str">The text to remove affixes from.</param>
    /// <param name="affixesToTrim">Zero or more affixes, all of which will be removed from <paramref name="str"/>.</param>
    /// <returns>A substring of <paramref name="str"/> that does neither starts nor ends with any of the <paramref name="affixesToTrim"/>.</returns>
    [Pure]
    public static string Trim(this ReadOnlySpan<char> str, params string[] affixesToTrim) {
        return TrimEnd(TrimStart(str, affixesToTrim), affixesToTrim);
    }

}